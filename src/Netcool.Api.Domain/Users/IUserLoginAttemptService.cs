using Netcool.Core.Services;
using Netcool.Core.Services.Dto;

namespace Netcool.Api.Domain.Users
{
    public interface IUserLoginAttemptService : ICrudService<UserLoginAttemptDto, int, PageRequest>
    {
    }
}