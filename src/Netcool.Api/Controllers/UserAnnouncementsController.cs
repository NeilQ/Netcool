using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Netcool.Core.Announcements;
using Netcool.Core.AspNetCore.Controllers;

namespace Netcool.Api.Controllers;

[Route("user-announcements")]
[Authorize]
public class UserAnnouncementsController : QueryControllerBase<UserAnnouncementDto, int, UserAnnouncementRequest>
{
    private new readonly IUserAnnouncementService Service;

    public UserAnnouncementsController(IUserAnnouncementService service) : base(service)
    {
        Service = service;
    }

    [Route("read")]
    [HttpPost]
    public async Task<IActionResult> ReadAsync(UserAnnouncementReadInput input)
    {
        await Service.ReadAsync(input);
        return Ok();
    }
}
