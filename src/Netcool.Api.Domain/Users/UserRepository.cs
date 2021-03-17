using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using Netcool.Api.Domain.EfCore;
using Netcool.Api.Domain.Permissions;
using Netcool.Api.Domain.Repositories;
using Netcool.Core.Repositories;

namespace Netcool.Api.Domain.Users
{
    public interface IUserRepository : IRepository<User>
    {
        List<Permission> GetUserPermissions(int id);

        void ClearUserPermissionCache(int id);
    }

    public class UserRepository : CommonRepository<User>, IUserRepository
    {
        public override IQueryable<User> GetAll()
        {
            return GetAllIncluding(t => t.Organization);
        }

        private readonly IMemoryCache _cache;

        public UserRepository(NetcoolDbContext dbContext, IMemoryCache cache) : base(dbContext)
        {
            _cache = cache;
        }

        public List<Permission> GetUserPermissions(int id)
        {
            if (_cache.TryGetValue($"user-{id}-permissions", out List<Permission> permissions))
            {
                return permissions;
            }

            var user = ContextBase.Set<User>().AsNoTracking()
                .Include(t => t.UserRoles)
                .ThenInclude(t => t.Role)
                .ThenInclude(t => t.RolePermissions)
                .ThenInclude(t => t.Permission)
                .FirstOrDefault(t => t.Id == id);

            permissions = user?.UserRoles
                .SelectMany(t => t.Role.RolePermissions.Select(rp => rp.Permission))
                .Distinct()
                .ToList();
            _cache.Set($"user-{id}-permissions", permissions, TimeSpan.FromHours(2));
            return permissions;
        }

        public void ClearUserPermissionCache(int id)
        {
            _cache.Remove($"user-{id}-permissions");
        }
    }
}