using System.Linq.Dynamic.Core;
using Netcool.Api.Domain.Users;
using Netcool.Core.Helpers;

namespace Netcool.Api.Domain.EfCore
{
    public static class DbInitializer
    {
        public static void Initialize(NetcoolDbContext context)
        {
            if (!context.Users.Any())
            {
                context.Users.Add(new User
                {
                    Id = 1,
                    Name = "admin",
                    DisplayName = "Admin",
                    IsRoot = true,
                    IsActive = true,
                    Password = Encrypt.Md5By32("admin")
                });
                context.SaveChanges();
            }
        }
    }
}