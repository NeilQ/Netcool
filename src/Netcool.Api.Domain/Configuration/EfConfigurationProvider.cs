using System;
using System.Linq;
using System.Threading;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Netcool.Api.Domain.EfCore;
using Netcool.Core.AppSettings;
using Netcool.Core.EfCore;
using Netcool.Core.Sessions;

namespace Netcool.Api.Domain.Configuration
{
    public class EfConfigurationProvider : ConfigurationProvider
    {
        public Action<DbContextOptionsBuilder> OptionsAction { get; }

        public EfConfigurationProvider(Action<DbContextOptionsBuilder> optionsAction, bool reloadOnChange = true)
        {
            OptionsAction = optionsAction;

            if (reloadOnChange)
            {
                EntityChangeObserver.Instance.Changed += (sender, args) =>
                {
                    if (args.Entry.Entity.GetType() != typeof(AppConfiguration))
                        return;

                    // Waiting before calling Load. This helps avoid triggering a reload before a change is completely saved.
                    Thread.Sleep(3000);
                    Load();
                };
            }
        }

        public override void Load()
        {
            var builder = new DbContextOptionsBuilder<NetcoolDbContext>();

            OptionsAction(builder);

            using var dbContext = new NetcoolDbContext(builder.Options, NullUserSession.Instance);
            InitializeDefaultConfigurations(dbContext);

            Data = dbContext.AppConfigurations.ToDictionary(c => c.Name, c => c.Value);
        }

        public static void InitializeDefaultConfigurations(NetcoolDbContext dbContext)
        {
            var configs = dbContext.AppConfigurations.ToList();
            if (configs.All(t => t.Name != "User.DefaultPassword"))
            {
                dbContext.AddRange(new AppConfiguration()
                {
                    Name = "User.DefaultPassword",
                    Value = "123456",
                    Type = AppConfigurationType.String,
                    Description = "默认用户密码"
                });
            }

            dbContext.SaveChanges();
        }
    }
}