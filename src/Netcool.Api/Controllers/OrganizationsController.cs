using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Netcool.Core.AspNetCore.Controllers;
using Netcool.Core.Organizations;
using Netcool.Core.Services.Dto;

namespace Netcool.Api.Controllers;

[Route("organizations")]
[Authorize]
public class OrganizationsController : CrudControllerBase<OrganizationDto, int, PageRequest, OrganizationSaveInput>
{
    public OrganizationsController(
        IOrganizationService service) :
        base(service)
    {
    }
}