using System.Collections.Generic;
using Netcool.Api.Domain.Roles;
using Netcool.Core.Services;
using Netcool.Core.Services.Dto;

namespace Netcool.Api.Domain.Users
{
    public interface IUserService : ICrudService<UserDto, int, PageRequest, UserSaveInput>
    {
        public void ChangePassword(int id, ChangePasswordInput input);

        public void ResetPassword(int id, ResetPasswordInput input);

        IList<RoleDto> GetUserRoles(int id);
    }
}