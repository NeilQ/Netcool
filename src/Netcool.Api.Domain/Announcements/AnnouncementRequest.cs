namespace Netcool.Core.Announcements
{
    public class AnnouncementRequest
    {
        public string Title { get; set; }

        public AnnouncementStatus? Status { get; set; }

        public NotifyTargetType? NotifyTargetType { get; set; }
    }
}
