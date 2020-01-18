using AutoMapper;
using Netcool.Core.Sessions;

namespace Netcool.Core.Services
{
    public class ServiceAggregator : IServiceAggregator
    {
        public IUnitOfWork UnitOfWork { get; set; }
        public IUserSession Session { get; set; }
        public IMapper Mapper { get; set; }

        public ServiceAggregator()
        {
        }

        public ServiceAggregator(IUnitOfWork unitOfWork, IUserSession session, IMapper mapper)
        {
            UnitOfWork = unitOfWork;
            Session = session;
            Mapper = mapper;
        }
    }
}