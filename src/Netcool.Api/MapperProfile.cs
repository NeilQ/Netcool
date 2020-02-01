using AutoMapper;
using Netcool.Api.Domain.Permissions;
using Netcool.Api.Domain.Roles;
using Netcool.Api.Domain.Users;

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
        }
    }
}