using System.Threading.Tasks;
using Netcool.Core.Services;

namespace Netcool.Api.Domain.Authorization
{
    public interface ILoginService : IService
    {
        public Task<LoginResult> LoginAsync(LoginInput input);
    }
}
