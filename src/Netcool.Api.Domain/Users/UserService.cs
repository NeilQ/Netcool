using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Netcool.Api.Domain.Menus;
using Netcool.Api.Domain.Roles;
using Netcool.Core;
using Netcool.Core.Entities;
using Netcool.Core.Helpers;
using Netcool.Core.Repositories;
using Netcool.Core.Services;
using Netcool.Core.Services.Dto;

namespace Netcool.Api.Domain.Users
{
    public sealed class UserService : CrudService<User, UserDto, int, PageRequest, UserSaveInput>, IUserService
    {
        private readonly string _defaultPassword;

        private readonly IUserRepository _userRepository;
        private readonly IRepository<Role> _roleRepository;
        private readonly IRepository<UserRole> _userRoleRepository;

        public UserService(IUserRepository userRepository,
            IServiceAggregator serviceAggregator,
            IRepository<Role> roleRepository,
            IConfiguration config,
            IRepository<UserRole> userRoleRepository) : base(
            userRepository,
            serviceAggregator)
        {
            _roleRepository = roleRepository;
            _userRepository = userRepository;
            _userRoleRepository = userRoleRepository;
            _defaultPassword = config.GetValue<string>("User.DefaultPassword");

            GetPermissionName = "user.view";
            UpdatePermissionName = "user.update";
            CreatePermissionName = "user.create";
            DeletePermissionName = "user.delete";
        }

        public override PagedResultDto<UserDto> GetAll(PageRequest input)
        {
            var dto = base.GetAll(input);
            if (dto?.Items == null || dto.Items.Count <= 0) return dto;

            var userRoles = _userRoleRepository.GetAll().AsNoTracking()
                .Where(t => dto.Items.Select(u => u.Id).Contains(t.UserId))
                .Include(t => t.Role).ToList();
            foreach (var userDto in dto.Items)
            {
                userDto.Roles = userRoles.Where(t => t.UserId == userDto.Id)
                    .Select(t => MapToEntityDto<Role, RoleDto>(t.Role)).ToList();
            }

            return dto;
        }

        public override void BeforeCreate(User entity)
        {
            // initialize password
            base.BeforeCreate(entity);
            entity.Name = entity.Name.SafeString();
            entity.Phone = entity.Phone.SafeString();
            entity.Email = entity.Email.SafeString();
            entity.Password = Encrypt.Md5By32(_defaultPassword);

            // check email and phone
            var duplicateUser = Repository.GetAll().AsNoTracking()
                .FirstOrDefault(t =>
                    t.Name == entity.Name ||
                    !string.IsNullOrEmpty(t.Phone) && t.Phone == entity.Phone ||
                    !string.IsNullOrEmpty(t.Email) && t.Email == entity.Email);
            if (duplicateUser == null) return;
            if (duplicateUser.Name == entity.Name)
            {
                throw new UserFriendlyException("用户名已存在");
            }

            if (!string.IsNullOrEmpty(duplicateUser.Phone) && duplicateUser.Phone == entity.Phone)
            {
                throw new UserFriendlyException("手机号已存在");
            }

            if (!string.IsNullOrEmpty(duplicateUser.Email) && duplicateUser.Email == entity.Email)
            {
                throw new UserFriendlyException("邮箱已存在");
            }
        }

        public override void BeforeUpdate(UserSaveInput dto, User originEntity)
        {
            base.BeforeUpdate(dto, originEntity);
            dto.Name = dto.Name.SafeString();
            dto.Phone = dto.Phone.SafeString();
            dto.Email = dto.Email.SafeString();

            // check email and phone
            var duplicateUser = Repository.GetAll().AsNoTracking()
                .FirstOrDefault(t =>
                    t.Id != originEntity.Id &&
                    (t.Name == dto.Name ||
                     !string.IsNullOrEmpty(t.Phone) && t.Phone == dto.Phone ||
                     !string.IsNullOrEmpty(t.Email) && t.Email == dto.Email));
            if (duplicateUser == null) return;
            if (duplicateUser.Name == dto.Name)
            {
                throw new UserFriendlyException("用户名已存在");
            }

            if (!string.IsNullOrEmpty(duplicateUser.Phone) && duplicateUser.Phone == dto.Phone)
            {
                throw new UserFriendlyException("手机号已存在");
            }

            if (!string.IsNullOrEmpty(duplicateUser.Email) && duplicateUser.Email == dto.Email)
            {
                throw new UserFriendlyException("邮箱已存在");
            }
        }

        protected override void BeforeDelete(IEnumerable<int> ids)
        {
            base.BeforeDelete(ids);
            _userRoleRepository.Delete(t => ids.Contains(t.UserId));
        }

        public void ChangePassword(int id, ChangePasswordInput input)
        {
            input.Origin = input.Origin.SafeString();
            input.New = input.New.SafeString();
            input.Confirm = input.Confirm.SafeString();

            var user = GetEntityById(id);

            if (Encrypt.Md5By32(input.Origin) != user.Password)
            {
                throw new UserFriendlyException("原密码输入错误");
            }

            user.Password = Encrypt.Md5By32(input.New);
            UnitOfWork.SaveChanges();
        }

        public void ResetPassword(int id, ResetPasswordInput input)
        {
            CheckUpdatePermission();
            input.New = input.New.SafeString();
            input.Confirm = input.Confirm.SafeString();

            var user = GetEntityById(id);

            if (input.New != input.Confirm)
            {
                throw new UserFriendlyException("两次密码输入不匹配");
            }

            user.Password = Encrypt.Md5By32(input.New);
            UnitOfWork.SaveChanges();
        }

        public IList<RoleDto> GetUserRoles(int id)
        {
            if (id <= 0) throw new EntityNotFoundException(typeof(User), id);
            var user = Repository.GetAll().AsNoTracking()
                .Include(t => t.UserRoles)
                .ThenInclude(t => t.Role)
                .FirstOrDefault(t => t.Id == id);
            if (user == null) throw new EntityNotFoundException(typeof(User), id);
            return user.UserRoles?.Select(t => MapToEntityDto<Role, RoleDto>(t.Role)).ToList();
        }

        public void SetUserRoles(int id, IList<int> roleIds)
        {
            CheckPermission("user.set-roles");
            // validate user
            if (id <= 0) throw new EntityNotFoundException(typeof(User), id);
            var user = Repository.GetAll()
                .Include(t => t.UserRoles)
                .FirstOrDefault(t => t.Id == id);
            if (user == null) throw new EntityNotFoundException(typeof(User), id);

            // validate roleIds
            if (roleIds != null && roleIds.Count > 0)
            {
                var roles = _roleRepository.GetAll().AsNoTracking()
                    .Where(t => roleIds.Any(r => r == t.Id)).ToList();
                var invalidIds = roleIds.Where(t => roles.All(r => r.Id != t)).ToList();
                if (invalidIds.Count > 0)
                {
                    throw new EntityNotFoundException($"Role with ids [{string.Join(',', invalidIds)}] not found.");
                }
            }

            // save user roles
            user.UserRoles = new List<UserRole>();
            if (roleIds != null && roleIds.Count > 0)
            {
                foreach (var roleId in roleIds)
                {
                    user.UserRoles.Add(new UserRole(id, roleId));
                }
            }

            UnitOfWork.SaveChanges();
            _userRepository.ClearUserPermissionCache(id);
        }

        public MenuTreeNode GetUserMenuTree(int id)
        {
            if (id <= 0) throw new EntityNotFoundException(typeof(User), id);
            var user = Repository.GetAll().AsNoTracking()
                .Include(t => t.UserRoles)
                .ThenInclude(t => t.Role)
                .ThenInclude(t => t.RolePermissions)
                .ThenInclude(t => t.Permission)
                .ThenInclude(t => t.Menu)
                .FirstOrDefault(t => t.Id == id);
            if (user == null) throw new EntityNotFoundException(typeof(User), id);
            var menus = user.UserRoles.SelectMany(t => t.Role.RolePermissions.Select(rp => rp.Permission.Menu))
                .Distinct().OrderBy(t => t.Level).ToList();
            var rootNode = new MenuTreeNode();

            var dict = new Dictionary<int, MenuTreeNode>
            {
                {0, rootNode}
            };
            foreach (var menu in menus)
            {
                if (dict.ContainsKey(menu.Id)) continue; // ignore duplicate menu node
                var treeNode = MapToEntityDto<Menu, MenuTreeNode>(menu);
                dict.Add(menu.Id, treeNode);

                if (menu.ParentId > 0 && menu.Level > 1)
                {
                    if (!dict.TryGetValue(menu.ParentId, out var parentNode))
                        continue; // ignore menu node with invalid parentId

                    if (parentNode.Children == null) parentNode.Children = new List<MenuTreeNode>();
                    parentNode.Children.Add(treeNode);
                }
                else
                {
                    if (rootNode.Children == null) rootNode.Children = new List<MenuTreeNode>();
                    rootNode.Children.Add(treeNode);
                }
            }

            return rootNode;
        }
    }
}