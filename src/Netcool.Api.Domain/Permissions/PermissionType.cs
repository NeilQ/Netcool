using System.ComponentModel;

namespace Netcool.Api.Domain.Permissions
{
    public enum PermissionType
    {
        [Description("菜单权限")] Menu,

        [Description("功能权限")] Function
    }
}