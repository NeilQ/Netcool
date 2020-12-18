using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Netcool.Api.Domain.Configuration;
using Netcool.Core.Services.Dto;
using Netcool.Core.WebApi.Controllers;

namespace Netcool.Api.Controllers
{
    [Route("app-configurations")]
    [Authorize]
    public class AppConfigurationsController : CrudControllerBase<AppConfigurationDto, int, PageRequest,
        AppConfigurationSaveInput>
    {
        public AppConfigurationsController(IAppConfigurationService service) : base(service)
        {
        }
      
    }
}