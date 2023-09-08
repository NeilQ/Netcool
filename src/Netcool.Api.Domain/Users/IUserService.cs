using System.Collections.Generic;
using System.Threading.Tasks;
using Netcool.Api.Domain.Menus;
using Netcool.Api.Domain.Roles;
using Netcool.Core.Services;

namespace Netcool.Api.Domain.Users
{
    public interface IUserService : ICrudService<UserDto, int, UserRequest, UserSaveInput>
    {
        public Task ChangePasswordAsync(int id, ChangePasswordInput input);

        public Task ResetPasswordAsync(int id, ResetPasswordInput input);

        public IList<RoleDto> GetUserRoles(int id);

        public Task SetUserRolesAsync(int id, IList<int> roleIds);
        
        public MenuTreeNode GetUserMenuTree(int id);
    }
}
