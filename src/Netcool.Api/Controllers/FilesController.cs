using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.Net.Http.Headers;
using Netcool.Api.Domain.Files;
using Netcool.Core;
using Netcool.Core.WebApi;
using Netcool.Core.WebApi.Controllers;
using Netcool.Core.WebApi.Filters;
using ContentDispositionHeaderValue = System.Net.Http.Headers.ContentDispositionHeaderValue;

namespace Netcool.Api.Controllers
{
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
        public IActionResult Post([FromBody] FileActiveInput input)
        {
            _fileService.ActivePicture(input);
            return Ok();
        }

        /// <summary>
        /// base64方式上传文件
        /// </summary>
        /// <param name="upload"></param>
        /// <returns></returns>
        [HttpPost("upload/base64")]
        [RequestSizeLimit(1024 * 1024 * 50)] // 50M
        [Validate]
        public ActionResult<FileDto> UploadBase64([FromBody] PictureBase64Upload upload)
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

            var pictureDto = Service.Create(new FileSaveInput
            {
                Title = upload.Filename,
                Filename = $"{fileFolderName}/{fileName}",
                Description = upload.Description
            });
            return Ok(pictureDto);
        }


        /// <summary>
        /// multipart/form-data方式上传图片
        /// http data 示例：
        /// Content-Type: multipart/form-data; boundary="----WebKitFormBoundarymx2fSWqWSd0OxQqq"
        /// Content-Disposition: form-data; name="myFile1"; filename="Misc002.jpg"
        /// 支持的content-disposition: "form-data", "file", "attachment", "image"
        /// </summary>
        /// <returns></returns>
        [HttpPost("upload")]
        [ProducesResponseType(typeof(FileDto), 200)]
        [FileUploadOperationFilter.FileContentType]
        public async Task<IActionResult> Upload()
        {
            var customFilename = Request.Query["customFilename"].ToString().ToLower() == "true" ||
                               Request.Query["custom_filename"].ToString().ToLower() == "true";

            if (!MultipartRequestHelper.IsMultipartContentType(Request.ContentType))
            {
                return BadRequest($"Expected a multipart request, but got {Request.ContentType}");
            }

            var boundary = MultipartRequestHelper.GetBoundary(
                MediaTypeHeaderValue.Parse(Request.ContentType),
                _formOptions.Value.MultipartBoundaryLengthLimit);
            var reader = new MultipartReader(boundary, HttpContext.Request.Body);

            var section = await reader.ReadNextSectionAsync();
            if (section == null) return BadRequest("Invalid boundary");

            var hasContentDispositionHeader = ContentDispositionHeaderValue.TryParse(section.ContentDisposition,
                out ContentDispositionHeaderValue contentDisposition);

            if (!hasContentDispositionHeader)
            {
                return BadRequest("Invalid Content-Disposition header.");
            }

            if (!MultipartRequestHelper.HasFileContentDisposition(contentDisposition))
            {
                return BadRequest("Invalid Content-Disposition header value.");
            }

            var fileFolderName = DateTime.Now.ToString("yyyyMMdd");
            var fileFolderPath = Path.Combine(_fileOptions.Value.PhysicalPath, fileFolderName);
            if (!Directory.Exists(fileFolderPath))
            {
                Directory.CreateDirectory(fileFolderPath);
            }

            var originFileName = !string.IsNullOrEmpty(contentDisposition.FileName)
                ? contentDisposition.FileName.Replace("\"", "").Replace("\\", "")
                : contentDisposition.FileNameStar?.Replace("\"", "").Replace("\\", "");
            var fileName = customFilename && !string.IsNullOrEmpty(originFileName)
                ? originFileName
                : Guid.NewGuid() +
                  Path.GetExtension(originFileName);
            var filePath = Path.Combine(fileFolderPath, fileName);
            await using (var targetStream = System.IO.File.Create(filePath))
            {
                await section.Body.CopyToAsync(targetStream);

                _logger.LogInformation($"Copied the uploaded file '{filePath}'");
            }

            // Persist and return picture dto
            var fileDto = Service.Create(new FileSaveInput
            {
                Title = originFileName,
                Filename = $"{fileFolderName}/{fileName}"
                // Be careful about '/' & '\'. The '/' for url, and  the '\' for disk io path
            });
            return Ok(fileDto);
        }
    }
}