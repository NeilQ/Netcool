using System.Net;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Options;
using Microsoft.Net.Http.Headers;
using Netcool.Api.Domain.Files;
using Netcool.Core;
using Netcool.Core.AspNetCore;
using Netcool.Core.AspNetCore.Controllers;
using Netcool.Core.AspNetCore.Filters;
using Netcool.Swashbuckle.AspNetCore;
using ContentDispositionHeaderValue = System.Net.Http.Headers.ContentDispositionHeaderValue;

namespace Netcool.Api.Controllers;

[Route("files")]
[Authorize]
public class FilesController : CrudControllerBase<FileDto, int, FileQuery, FileSaveInput>
{
    private readonly IOptionsSnapshot<FormOptions> _formOptions;
    private readonly IOptionsSnapshot<FileUploadOptions> _fileOptions;
    private readonly ILogger _logger;
    private readonly IFileService _fileService;

    public FilesController(IFileService service,
        IOptionsSnapshot<FormOptions> formOptions,
        IOptionsSnapshot<FileUploadOptions> fileOptions,
        ILogger<FilesController> logger) :
        base(service)
    {
        _fileService = service;
        _formOptions = formOptions;
        _fileOptions = fileOptions;
        _logger = logger;
    }

    /// <summary>
    /// 将文件设置为有效
    /// </summary>
    /// <returns></returns>
    [HttpPost("active")]
    [Validate]
    public async Task<IActionResult> Post([FromBody] FileActiveInput input)
    {
        await _fileService.ActiveAsync(input);
        return Ok();
    }


    [HttpPost("upload/wang-editor")]
    [RequestSizeLimit(1024 * 1024 * 30)] // 50M
    [FileUploadOperationFilter.FileContentType]
    public async Task<ActionResult> UploadWangEditorAsync()
    {
        var fileDto = await UploadMultipartAsync();
        return Ok(new
        {
            errno = 0,
            data = new[]
            {
                new
                {
                    url = fileDto.Url,
                    alt = fileDto.Title,
                    href = fileDto.Url
                }
            }
        });
    }

    /// <summary>
    /// base64方式上传文件
    /// </summary>
    /// <param name="upload"></param>
    /// <returns></returns>
    [HttpPost("upload/base64")]
    [RequestSizeLimit(1024 * 1024 * 50)] // 50M
    [Validate]
    public async Task<ActionResult<FileDto>> UploadBase64Async([FromBody] PictureBase64Upload upload)
    {
        var fileFolderName = DateTime.Now.ToString("yyyyMMdd");
        var fileFolderPath = Path.Combine(_fileOptions.Value.PhysicalPath, fileFolderName);
        if (!Directory.Exists(fileFolderPath))
        {
            Directory.CreateDirectory(fileFolderPath);
        }

        var fileName = upload.KeepFileName && !string.IsNullOrEmpty(upload.Filename)
            ? upload.Filename
            : Guid.NewGuid() +
              Path.GetExtension(upload.Filename);
        var filePath = Path.Combine(fileFolderPath, fileName);
        var bytes = Convert.FromBase64String(upload.Base64);
        if (System.IO.File.Exists(filePath))
            throw new UserFriendlyException($"文件[{filePath}]已存在");

        try
        {
            using var imageFile = new FileStream(filePath, FileMode.CreateNew);
            imageFile.Write(bytes, 0, bytes.Length);
            imageFile.Flush();
        }
        catch (Exception e)
        {
            _logger.LogError($"文件保存到磁盘失败，{e.Message}");
            throw;
        }

        var pictureDto = await Service.CreateAsync(new FileSaveInput
        {
            Title = upload.Filename,
            Filename = $"{fileFolderName}/{fileName}",
            Description = upload.Description
        });
        return Ok(pictureDto);
    }


    /// <summary>
    /// multipart/form-data方式上传文件
    /// http data 示例：
    /// Content-Type: multipart/form-data; boundary="----WebKitFormBoundarymx2fSWqWSd0OxQqq"
    /// Content-Disposition: form-data; name="myFile1"; filename="Misc002.jpg"
    /// 支持的content-disposition: "form-data", "file", "attachment", "image"
    /// </summary>
    /// <returns></returns>
    [HttpPost("upload")]
    [ProducesResponseType(typeof(FileDto), 200)]
    [FileUploadOperationFilter.FileContentType]
    public async Task<IActionResult> UploadAsync()
    {
        var fileDto = await UploadMultipartAsync();
        return Ok(fileDto);
    }

    private async Task<FileDto> UploadMultipartAsync()
    {
        var customFilename = Request.Query["customFilename"].ToString().ToLower() == "true" ||
                             Request.Query["custom_filename"].ToString().ToLower() == "true";

        if (!MultipartRequestHelper.IsMultipartContentType(Request.ContentType))
        {
            throw new ArgumentException($"Expected a multipart request, but got {Request.ContentType}");
        }

        var boundary = MultipartRequestHelper.GetBoundary(
            MediaTypeHeaderValue.Parse(Request.ContentType),
            _formOptions.Value.MultipartBoundaryLengthLimit);
        var reader = new MultipartReader(boundary, HttpContext.Request.Body);

        var section = await reader.ReadNextSectionAsync();
        if (section == null) throw new ApplicationException("Invalid boundary");

        var hasContentDispositionHeader = ContentDispositionHeaderValue.TryParse(section.ContentDisposition,
            out ContentDispositionHeaderValue contentDisposition);

        if (!hasContentDispositionHeader)
        {
            throw new ArgumentException("Invalid Content-Disposition header.");
        }

        if (!MultipartRequestHelper.HasFileContentDisposition(contentDisposition))
        {
            throw new ArgumentException("Invalid Content-Disposition header value.");
        }

        var fileFolderName = DateTime.Now.ToString("yyyyMMdd");
        var fileFolderPath = Path.Combine(_fileOptions.Value.PhysicalPath, fileFolderName);
        if (!Directory.Exists(fileFolderPath))
        {
            Directory.CreateDirectory(fileFolderPath);
        }

        var originFileName = WebUtility.HtmlEncode(!string.IsNullOrEmpty(contentDisposition.FileName)
            ? contentDisposition.FileName.Replace("\"", "").Replace("\\", "")
            : contentDisposition.FileNameStar?.Replace("\"", "").Replace("\\", ""));
        var fileName = customFilename && !string.IsNullOrEmpty(originFileName)
            ? originFileName
            : Path.GetRandomFileName() + Path.GetExtension(originFileName);
        var filePath = Path.Combine(fileFolderPath, fileName);
        await using (var targetStream = System.IO.File.Create(filePath))
        {
            await section.Body.CopyToAsync(targetStream);

            _logger.LogInformation($"Copied the uploaded file '{filePath}'");
        }

        // Persist and return file dto
        var fileDto = await Service.CreateAsync(new FileSaveInput
        {
            Title = originFileName,
            Filename = $"{fileFolderName}/{fileName}"
            // Be careful about '/' & '\'. The '/' for url, and  the '\' for disk io path
        });
        return fileDto;
    }
}
