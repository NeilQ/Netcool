using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
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
        private const string DefaultPassword = "123456";

        public UserService(IRepository<User> userRepository, IServiceAggregator serviceAggregator) : base(
            userRepository,
            serviceAggregator)
        {
        }

        public override void BeforeCreate(User entity)
        {
            // initialize password
            entity.Name = entity.Name.SafeString();
            entity.Phone = entity.Phone.SafeString();
            entity.Password = Encrypt.Md5By32(DefaultPassword);
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
            var user = Repository.GetAll().AsNoTracking()
                .Include(t => t.UserRoles)
                .ThenInclude(t => t.Role)
                .FirstOrDefault(t => t.Id == id);
            if (user == null) throw new EntityNotFoundException(typeof(User), id);
            return user.UserRoles?.Select(t => MapToEntityDto<Role, RoleDto>(t.Role)).ToList();
        }
    }
}