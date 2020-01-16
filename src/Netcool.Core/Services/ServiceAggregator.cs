using AutoMapper;
using Netcool.Core.Sessions;

namespace Netcool.Core.Services
{
    public class ServiceAggregator : IServiceAggregator
    {
        public IUnitOfWork UnitOfWork { get; set; }
        public INetcoolSession Session { get; set; }
        public IMapper Mapper { get; set; }

        public ServiceAggregator()
        {
        }

        public ServiceAggregator(IUnitOfWork unitOfWork, INetcoolSession session, IMapper mapper)
        {
            UnitOfWork = unitOfWork;
            Session = session;
            Mapper = mapper;
        }
    }
}