using Netcool.Core.Services;

namespace Netcool.Api.Domain.Configuration
{
    public interface
        IAppConfigurationService : ICrudService<AppConfigurationDto, int, AppConfigurationRequest, AppConfigurationSaveInput>
    {
    }
}