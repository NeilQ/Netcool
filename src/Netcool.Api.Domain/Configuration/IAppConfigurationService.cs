using Netcool.Core.Services;
using Netcool.Core.Services.Dto;

namespace Netcool.Api.Domain.Configuration
{
    public interface
        IAppConfigurationService : ICrudService<AppConfigurationDto, int, AppConfigurationRequest, AppConfigurationSaveInput>
    {
    }
}