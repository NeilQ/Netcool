using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using Netcool.Api.Domain.Configuration;
using Netcool.Api.Domain.Files;
using Netcool.Api.Domain.Menus;
using Netcool.Api.Domain.Permissions;
using Netcool.Api.Domain.Roles;
using Netcool.Api.Domain.Users;
using Netcool.Core.Extensions;

namespace Netcool.Api
{
    public class MapperProfile : Profile
    {
        public MapperProfile()
        {
            CreateMap<User, UserDto>();
            CreateMap<UserSaveInput, User>();
            CreateMap<Role, RoleDto>();
            CreateMap<RoleSaveInput, Role>();
            CreateMap<Permission, PermissionDto>();
            CreateMap<UserLoginAttempt, UserLoginAttemptDto>();
            CreateMap<AppConfiguration, AppConfigurationDto>();
            CreateMap<AppConfigurationSaveInput, AppConfiguration>();
            CreateMap<Menu, MenuDto>();
            CreateMap<Menu, MenuTreeNode>();
            CreateMap<MenuDto, Menu>();
            CreateMap<FileSaveInput, File>();
            CreateMap<File, FileDto>()
                .ForMember(s => s.Host, opts => opts.MapFrom<HostResolver>())
                .ForMember(s => s.Url, opts => opts.MapFrom<FileUrlResolver>());
        }
    }

    public class HostResolver : IValueResolver<File, FileDto, string>
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IOptionsSnapshot<FileUploadOptions> _fileOptions;

        public HostResolver(IHttpContextAccessor httpContextAccessor, IOptionsSnapshot<FileUploadOptions> fileOptions)
        {
            _httpContextAccessor = httpContextAccessor;
            _fileOptions = fileOptions;
        }

        public string Resolve(File source, FileDto destination, string destMember, ResolutionContext context)
        {
            var host = !string.IsNullOrWhiteSpace(_fileOptions.Value.Host)
                ? _fileOptions.Value.Host
                : _httpContextAccessor.HttpContext?.Request.Host.Value;
            return host;
        }
    }

    public class FileUrlResolver : IValueResolver<File, FileDto, string>
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IOptionsSnapshot<FileUploadOptions> _fileOptions;

        public FileUrlResolver(IHttpContextAccessor httpContextAccessor,
            IOptionsSnapshot<FileUploadOptions> fileOptions)
        {
            _httpContextAccessor = httpContextAccessor;
            _fileOptions = fileOptions;
        }

        public string Resolve(File source, FileDto destination, string destMember, ResolutionContext context)
        {
            var host = !string.IsNullOrWhiteSpace(_fileOptions.Value.Host)
                ? _fileOptions.Value.Host
                : _httpContextAccessor.HttpContext?.Request.Host.Value;
            var url = AppendUrlHost(host, _fileOptions.Value.SubWebPath, source.Filename);
            return url;
        }

        private string AppendUrlHost(string hostName, string subWebPath, string filename)
        {
            if (string.IsNullOrWhiteSpace(filename) || string.IsNullOrWhiteSpace(hostName)) return filename;
            if (filename.IsValidUrl()) return filename;
            return $"http://{hostName}/{subWebPath}/{filename.Trim('/')}";
        }
    }
}