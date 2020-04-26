using System.Collections.Generic;
using System.Linq;
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
    public class UserService : CrudService<User, UserDto, int, PageRequest, UserSaveInput>, IUserService
    {
        private readonly string _defaultPassword;

        private readonly IRepository<Role> _roleRepository;
        private readonly IRepository<UserRole> _userRoleRepository;

        public UserService(IRepository<User> userRepository,
            IServiceAggregator serviceAggregator,
            IRepository<Role> roleRepository,
            IConfiguration config,
            IRepository<UserRole> userRoleRepository) : base(
            userRepository,
            serviceAggregator)
        {
            _roleRepository = roleRepository;
            _userRoleRepository = userRoleRepository;
            _defaultPassword = config.GetValue<string>("User.DefaultPassword");
        }

        public override void BeforeCreate(User entity)
        {
            // initialize password
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