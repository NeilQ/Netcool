using MapsterMapper;
using Microsoft.AspNetCore.Authorization;
using Netcool.Core.Sessions;

namespace Netcool.Core.Services
{
    public interface IServiceAggregator
    {
        IUnitOfWork UnitOfWork { get; set; }
        ICurrentUser Session { get; set; }
        IMapper Mapper { get; set; }
        IAuthorizationService AuthorizationService { get; set; }
    }
}
