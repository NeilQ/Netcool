using Netcool.Core.Services.Dto;

namespace Netcool.Core.Announcements
{
    public class AnnouncementRequest : PageRequest
    {
        public string Title { get; set; }

        public AnnouncementStatus? Status { get; set; }

        public NotifyTargetType? NotifyTargetType { get; set; }
    }
}
