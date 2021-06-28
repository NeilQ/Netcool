using System;
using Netcool.Api.Domain.Users;
using Netcool.Core.Entities;

namespace Netcool.Core.Announcements
{
    public class UserAnnouncement : Entity
    {
        public int AnnouncementId { get; set; }

        public Announcement Announcement { get; set; }

        public int UserId { get; set; }

        public User User { get; set; }

        public bool IsRead { get; set; }

        public DateTime? ReadTime { get; set; }
    }
}
