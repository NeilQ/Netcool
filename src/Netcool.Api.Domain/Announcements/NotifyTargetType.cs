using System.ComponentModel;

namespace Netcool.Core.Announcements
{
    public enum NotifyTargetType
    {
        [Description("所有用户")] AllUsers,

        [Description("活跃用户")] ActiveUsers,
    }
}
