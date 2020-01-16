using AutoMapper;
using Netcool.Core.Sessions;

namespace Netcool.Core.Services
{
    public interface IServiceAggregator
    {
        
         IUnitOfWork UnitOfWork { get; set; }
         INetcoolSession Session { get; set; }
         IMapper Mapper { get; set; }
    }
}