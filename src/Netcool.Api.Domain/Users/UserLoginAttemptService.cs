using Netcool.Core.Repositories;
using Netcool.Core.Services;
using Netcool.Core.Services.Dto;

namespace Netcool.Api.Domain.Users
{
    public class UserLoginAttemptService : CrudService<UserLoginAttempt, UserLoginAttemptDto, int, PageRequest>,
        IUserLoginAttemptService
    {
        public UserLoginAttemptService(IRepository<UserLoginAttempt> repository, IServiceAggregator serviceAggregator) :
            base(repository, serviceAggregator)
        {
        }
    }
}