using Netcool.Core.Repositories;
using Netcool.Core.Services;
using Netcool.Core.Services.Dto;

namespace Netcool.Api.Domain.Configuration
{
    public class AppConfigurationService :
        CrudService<AppConfiguration, AppConfigurationDto, int, PageRequest, AppConfigurationSaveInput>,
        IAppConfigurationService
    {
        public AppConfigurationService(IRepository<AppConfiguration> repository, IServiceAggregator serviceAggregator) :
            base(repository, serviceAggregator)
        {
        }
    }
}