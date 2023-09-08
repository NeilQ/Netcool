using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Netcool.Api.Domain.Permissions;
using Netcool.Api.Domain.Users;
using Netcool.Core;
using Netcool.Core.Entities;
using Netcool.Core.Repositories;
using Netcool.Core.Services;

namespace Netcool.Api.Domain.Roles
{
    public sealed class RoleService : CrudService<Role, RoleDto, int, RoleRequest, RoleSaveInput>, IRoleService
    {
        private readonly IRepository<Permission> _permissionRepository;
        private readonly IRepository<UserRole> _userRoleRepository;
        private readonly IUserRepository _userRepository;

        public RoleService(IRepository<Role> repository,
            IServiceAggregator serviceAggregator,
            IRepository<Permission> permissionRepository,
            IRepository<UserRole> userRoleRepository, IHttpContextAccessor accessor,
            IUserRepository userRepository) : base(repository,
            serviceAggregator)
        {
            _permissionRepository = permissionRepository;
            _userRoleRepository = userRoleRepository;
            _userRepository = userRepository;

            GetPermissionName = "role.view";
            UpdatePermissionName = "role.update";
            CreatePermissionName = "role.create";
            DeletePermissionName = "role.delete";
        }

        protected override IQueryable<Role> CreateFilteredQuery(RoleRequest input)
        {
            var query = base.CreateFilteredQuery(input);
            if (input.Name != null)
            {
                query = query.Where(t => t.Name == input.Name);
            }

            return query;
        }

        public override void BeforeCreate(Role entity)
        {
            base.BeforeCreate(entity);
            var nameExist = Repository.GetAll().AsNoTracking().Any(t => t.Name == entity.Name);
            if (nameExist)
            {
                throw new UserFriendlyException("名称重复");
            }
        }

        public override void BeforeUpdate(RoleSaveInput input, Role originEntity)
        {
            base.BeforeUpdate(input, originEntity);
            var nameExist = Repository.GetAll().AsNoTracking()
                .Any(t => t.Name == input.Name && t.Id != originEntity.Id);
            if (nameExist)
            {
                throw new UserFriendlyException("名称重复");
            }
        }

        public IList<PermissionDto> GetRolePermissions(int id)
        {
            if (id <= 0) throw new EntityNotFoundException(typeof(Role), id);
            var role = Repository.GetAll().AsNoTracking()
                .Include(t => t.RolePermissions)
                .ThenInclude(t => t.Permission)
                .FirstOrDefault(t => t.Id == id);
            if (role == null) throw new EntityNotFoundException(typeof(Role), id);
            return role.RolePermissions?.Select(t => MapToEntityDto<Permission, PermissionDto>(t.Permission)).ToList();
        }

        public async Task SetRolePermissionsAsync(int id, IList<int> permissionIds)
        {
            CheckPermission("role.set-permissions");
            // validate role 
            if (id <= 0) throw new EntityNotFoundException(typeof(User), id);
            var role = Repository.GetAll()
                .Include(t => t.RolePermissions)
                .Include(t => t.UserRoles)
                .FirstOrDefault(t => t.Id == id);
            if (role == null) throw new EntityNotFoundException(typeof(Role), id);

            // validate permissionIds 
            if (permissionIds is { Count: > 0 })
            {
                var permissions = _permissionRepository.GetAll().AsNoTracking()
                    .Where(t => permissionIds.Any(p => p == t.Id)).ToList();
                var invalidIds = permissionIds.Where(t => permissions.All(r => r.Id != t)).ToList();
                if (invalidIds.Count > 0)
                {
                    throw new EntityNotFoundException(
                        $"Permission with ids [{string.Join(',', invalidIds)}] not found.");
                }
            }

            // save role permissions
            role.RolePermissions = new List<RolePermission>();
            if (permissionIds != null && permissionIds.Count > 0)
            {
                foreach (var permissionId in permissionIds)
                {
                    role.RolePermissions.Add(new RolePermission(id, permissionId));
                }
            }

            await UnitOfWork.SaveChangesAsync();

            foreach (var userRole in role.UserRoles)
            {
                _userRepository.ClearUserPermissionCache(userRole.UserId);
            }
        }

        protected override void BeforeDelete(IEnumerable<int> ids)
        {
            base.BeforeDelete(ids);
            var userRoles = _userRoleRepository.GetAll().AsNoTracking().Where(t => ids.Contains(t.RoleId)).ToList();
            foreach (var userRole in userRoles)
            {
                _userRepository.ClearUserPermissionCache(userRole.UserId);
            }

            _userRoleRepository.Delete(userRoles);
            // keep role permissions for deletion mistake temporarily
            // _rolePermissionRepository.Delete(t => ids.Contains(t.RoleId));
        }
    }
}
