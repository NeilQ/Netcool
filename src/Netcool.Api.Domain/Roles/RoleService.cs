using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Netcool.Api.Domain.Permissions;
using Netcool.Api.Domain.Users;
using Netcool.Core;
using Netcool.Core.Entities;
using Netcool.Core.Repositories;
using Netcool.Core.Services;
using Netcool.Core.Services.Dto;

namespace Netcool.Api.Domain.Roles
{
    public class RoleService : CrudService<Role, RoleDto, int, PageRequest, RoleSaveInput>, IRoleService
    {
        private readonly IRepository<Permission> _permissionRepository;

        public RoleService(IRepository<Role> repository,
            IServiceAggregator serviceAggregator,
            IRepository<Permission> permissionRepository) : base(repository, serviceAggregator)
        {
            _permissionRepository = permissionRepository;
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

        public void SetRolePermissions(int id, IList<int> permissionIds)
        {
            // validate role 
            if (id <= 0) throw new EntityNotFoundException(typeof(User), id);
            var role = Repository.GetAll()
                .Include(t => t.RolePermissions)
                .FirstOrDefault(t => t.Id == id);
            if (role == null) throw new EntityNotFoundException(typeof(Role), id);

            // validate permissionIds 
            if (permissionIds != null && permissionIds.Count > 0)
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

            UnitOfWork.SaveChanges();
        }
    }
}