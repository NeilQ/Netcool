using System;
using Netcool.Core.Services.Dto;

namespace Netcool.Core.Announcements
{
    public class UserAnnouncementDto : EntityDto
    {
        public AnnouncementDto Announcement { get; set; }

        public int UserId { get; set; }

        public bool IsRead { get; set; }

        public DateTime? ReadTime { get; set; }
    }
}
