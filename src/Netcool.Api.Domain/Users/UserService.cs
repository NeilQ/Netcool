using Netcool.Core.Helpers;
using Netcool.Core.Repositories;
using Netcool.Core.Services;
using Netcool.Core.Services.Dto;

namespace Netcool.Api.Domain.Users
{
    public class UserService : CrudService<User, UserDto, int, PageRequest, UserSaveInput>, IUserService
    {
        private const string DefaultPassword = "123456";

        public UserService(IRepository<User> repository, IServiceAggregator serviceAggregator) : base(repository,
            serviceAggregator)
        {
        }

        public override void BeforeCreate(User entity)
        {
            // initialize password
            entity.Password = Encrypt.Md5By32(DefaultPassword);
        }
    }
}