using Microsoft.Extensions.Logging;
using Netcool.Core.Sessions;

namespace Netcool.Core.Services
{
    public class ServiceBase : IService
    {
        private ILogger _logger;
        private INetcoolSession _session;
        private IUnitOfWork _uow;

        public ServiceBase(ILogger logger, INetcoolSession session, IUnitOfWork uow)
        {
            _logger = logger;
            _session = session;
            _uow = uow;
        }
    }
}