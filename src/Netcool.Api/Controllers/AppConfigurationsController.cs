using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Netcool.Api.Domain.Configuration;
using Netcool.Core.AspNetCore.Controllers;

namespace Netcool.Api.Controllers;

[Route("app-configurations")]
[Authorize]
public class AppConfigurationsController : CrudControllerBase<AppConfigurationDto, int, AppConfigurationRequest,
    AppConfigurationSaveInput>
{
    public AppConfigurationsController(IAppConfigurationService service) : base(service)
    {
    }
      
}