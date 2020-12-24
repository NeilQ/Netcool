using System.Collections.Generic;
using Netcool.Api.Domain.Menus;
using Netcool.Api.Domain.Roles;
using Netcool.Core.Services;

namespace Netcool.Api.Domain.Users
{
    public interface IUserService : ICrudService<UserDto, int, UserRequest, UserSaveInput>
    {
        public void ChangePassword(int id, ChangePasswordInput input);

        public void ResetPassword(int id, ResetPasswordInput input);

        public IList<RoleDto> GetUserRoles(int id);

        public void SetUserRoles(int id, IList<int> roleIds);
        
        public MenuTreeNode GetUserMenuTree(int id);
    }
}