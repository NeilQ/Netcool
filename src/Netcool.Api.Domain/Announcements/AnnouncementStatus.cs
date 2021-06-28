using System.ComponentModel;

namespace Netcool.Core.Announcements
{
    public enum AnnouncementStatus
    {
        [Description("草稿")] Draft,

        [Description("已发布")] Published
    }
}
