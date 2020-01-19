using AutoMapper;
using Netcool.Core.Authorization;
using Netcool.Core.Sessions;

namespace Netcool.Core.Services
{
    public class ServiceAggregator : IServiceAggregator
    {
        public IUnitOfWork UnitOfWork { get; set; }
        public IUserSession Session { get; set; }
        public IMapper Mapper { get; set; }
        public IPermissionChecker PermissionChecker { get; set; }

        public ServiceAggregator()
        {
        }

        public ServiceAggregator(IUnitOfWork unitOfWork, IUserSession session, IMapper mapper,
            IPermissionChecker permissionChecker)
        {
            UnitOfWork = unitOfWork;
            Session = session;
            Mapper = mapper;
            PermissionChecker = permissionChecker;
        }
    }
}