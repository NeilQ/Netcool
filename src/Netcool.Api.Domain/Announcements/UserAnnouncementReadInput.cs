using System.Collections.Generic;

namespace Netcool.Core.Announcements
{
    public class UserAnnouncementReadInput
    {
        public int UserId { get; set; }

        public List<int> AnnouncementIds { get; set; }
    }
}
