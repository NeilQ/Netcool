using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using Netcool.Api.Domain.Files;
using Netcool.Core;

namespace Netcool.Api;

public class FileUrlResolver
{
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly IOptionsSnapshot<FileUploadOptions> _fileOptions;

    public FileUrlResolver(IHttpContextAccessor httpContextAccessor,
        IOptionsSnapshot<FileUploadOptions> fileOptions)
    {
        _httpContextAccessor = httpContextAccessor;
        _fileOptions = fileOptions;
    }

    public string Resolve(File source)
    {
        var host = !string.IsNullOrWhiteSpace(_fileOptions.Value.Host)
            ? _fileOptions.Value.Host
            : _httpContextAccessor.HttpContext?.Request.Host.Value;
        var schema = !string.IsNullOrWhiteSpace(_fileOptions.Value.HostSchema)
            ? _fileOptions.Value.HostSchema
            : _httpContextAccessor.HttpContext?.Request.Scheme;
        var url = AppendUrlHost(schema, host, _fileOptions.Value.SubWebPath, source.Filename) + "?id=" +
                  source.Id;
        return url;
    }

    private string AppendUrlHost(string hostSchema, string hostName, string subWebPath, string filename)
    {
        if (string.IsNullOrWhiteSpace(filename) || string.IsNullOrWhiteSpace(hostName)) return filename;
        if (filename.IsValidUrl()) return filename;
        return $"{hostSchema}://{hostName}/{subWebPath}/{filename.Trim('/')}";
    }
}
