using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Netcool.Core.Announcements;
using Netcool.Core.WebApi.Controllers;

namespace Netcool.Api.Controllers
{
    [Route("user-announcements")]
    [Authorize]
    public class UserAnnouncementsController : QueryControllerBase<UserAnnouncementDto, int, UserAnnouncementRequest>
    {
        public UserAnnouncementsController(IUserAnnouncementService service) : base(service)
        {
        }
    }
}
