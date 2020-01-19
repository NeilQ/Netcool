using System.Threading.Tasks;

namespace Netcool.Core.Authorization
{
    /// <summary>
    /// This class is used to permissions for users.
    /// </summary>
    public interface IPermissionChecker
    {
        /// <summary>
        /// Checks if current user is granted for a permission.
        /// </summary>
        /// <param name="permissionName">Name of the permission</param>
        Task<bool> IsGrantedAsync(string permissionName);

        /// <summary>
        /// Checks if current user is granted for a permission.
        /// </summary>
        /// <param name="permissionName">Name of the permission</param>
        bool IsGranted(string permissionName);

        /// <summary>
        /// Checks if a user is granted for a permission.
        /// </summary>
        /// <param name="permissionName">Name of the permission</param>
        /// <param name="userId"></param>
        /// <param name="tenantId"></param>
        Task<bool> IsGrantedAsync(string permissionName, int userId, int? tenantId = null);

        /// <summary>
        /// Checks if a user is granted for a permission.
        /// </summary>
        /// <param name="permissionName">Name of the permission</param>
        /// <param name="userId"></param>
        /// <param name="tenantId"></param>
        bool IsGranted(string permissionName, int userId, int? tenantId = null);
    }
}