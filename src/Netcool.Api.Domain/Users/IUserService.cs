using Netcool.Core.Services;
using Netcool.Core.Services.Dto;

namespace Netcool.Api.Domain.Users
{
    public interface IUserService : ICrudService<UserDto, int, PageRequest, UserSaveInput>
    {
    }
}