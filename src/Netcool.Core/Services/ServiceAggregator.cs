using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Netcool.Core.Sessions;

namespace Netcool.Core.Services
{
    public class ServiceAggregator : IServiceAggregator
    {
        public IUnitOfWork UnitOfWork { get; set; }
        public IUserSession Session { get; set; }
        public IMapper Mapper { get; set; }
        public IAuthorizationService AuthorizationService { get; set; }

        public ServiceAggregator(IUnitOfWork unitOfWork, IUserSession session, IMapper mapper,
            IAuthorizationService authorizationService)
        {
            UnitOfWork = unitOfWork;
            Session = session;
            Mapper = mapper;
            AuthorizationService = authorizationService;
        }
    }
}