using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using Netcool.Api.Domain.Files;

namespace Netcool.Api;

public class FileHostResolver
{
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly IOptionsSnapshot<FileUploadOptions> _fileOptions;

    public FileHostResolver(IHttpContextAccessor httpContextAccessor,
        IOptionsSnapshot<FileUploadOptions> fileOptions)
    {
        _httpContextAccessor = httpContextAccessor;
        _fileOptions = fileOptions;
    }

    public string Resolve()
    {
        var host = !string.IsNullOrWhiteSpace(_fileOptions.Value.Host)
            ? _fileOptions.Value.Host
            : _httpContextAccessor.HttpContext?.Request.Host.Value;
        return host;
    }
}
