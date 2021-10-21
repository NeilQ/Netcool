using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Netcool.Core.Announcements;
using Netcool.Core.AspNetCore.Controllers;

namespace Netcool.Api.Controllers
{
    [Route("announcements")]
    [Authorize]
    public class AnnouncementsController :
        CrudControllerBase<AnnouncementDto, int, AnnouncementRequest, AnnouncementSaveInput>
    {
        private new readonly IAnnouncementService Service;

        public AnnouncementsController(IAnnouncementService service) : base(service)
        {
            Service = service;
        }

        [HttpPut("{id}/publish")]
        public IActionResult Publish(int id)
        {
            Service.Publish(id);
            return Ok();
        }
    }
}
