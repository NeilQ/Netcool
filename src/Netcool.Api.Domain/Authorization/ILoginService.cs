using Netcool.Core.Services;

namespace Netcool.Api.Domain.Authorization
{
    public interface ILoginService : IService
    {
        public LoginResult Login(LoginInput input);
    }
}