using Microsoft.AspNetCore.Mvc;
using Netcool.Api.Domain.Configuration;
using Netcool.Core.Services.Dto;
using Netcool.Core.WebApi.Controllers;

namespace Netcool.Api.Controllers
{
    [Route("app-configurations")]
    public class AppConfigurationsController : QueryControllerBase<AppConfigurationDto, int, PageRequest,
        AppConfigurationSaveInput>
    {
        public AppConfigurationsController(IAppConfigurationService service) : base(service)
        {
        }

        [HttpPut("{id}")]
        public IActionResult Update(int id, AppConfigurationSaveInput input)
        {
            input.Id = id;
            Service.Update(input);
            return Ok();
        }
    }
}