using Netcool.Core.Services.Dto;

namespace Netcool.Core.Announcements
{
    public class UserAnnouncementRequest : PageRequest
    {
        public int? UserId { get; set; }

        public bool? IsRead { get; set; }

        public AnnouncementStatus? AnnouncementStatus { get; set; }
    }
}
