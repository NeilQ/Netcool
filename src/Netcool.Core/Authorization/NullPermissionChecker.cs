using System.Threading.Tasks;

namespace Netcool.Core.Authorization
{
    public class NullPermissionChecker : IPermissionChecker
    {
        public Task<bool> IsGrantedAsync(string permissionName)
        {
            return Task.FromResult(true);
        }

        public bool IsGranted(string permissionName)
        {
            return true;
        }

        public Task<bool> IsGrantedAsync(string permissionName, int userId, int? tenantId = null)
        {
            return Task.FromResult(true);
        }

        public bool IsGranted(string permissionName, int userId, int? tenantId = null)
        {
            return true;
        }
    }
}