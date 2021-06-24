using System.ComponentModel.DataAnnotations;
using Netcool.Core.Entities;

namespace Netcool.Core.Announcements
{
    public class Announcement : FullAuditEntity
    {
        [StringLength(32, MinimumLength = 1)]
        public string Title { get; set; }

        public string Body { get; set; }

        public AnnouncementStatus Status { get; set; }

        public NotifyTargetType NotifyTargetType { get; set; }
    }
}
