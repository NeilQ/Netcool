using System;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Netcool.Api.Domain.EfCore;
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
                EntityChangeObserver.Instance.Changed += OnConfigChanged;
            }
        }

        public override void Load()
        {
            var builder = new DbContextOptionsBuilder<NetcoolDbContext>();

            OptionsAction(builder);

            using var dbContext = new NetcoolDbContext(builder.Options, NullCurrentUser.Instance);

            var configs = dbContext.AppConfigurations.ToList();
            Data.Clear();
            if (configs.Count <= 0) return;
            foreach (var config in configs)
            {
                if (string.IsNullOrEmpty(config.Name) || Data.ContainsKey(config.Name)) continue;
                Data.Add(config.Name, config.Value);
            }
        }

        private void OnConfigChanged(object sender, EntityChangeEvent args)
        {
            if (!(args.Entity is AppConfiguration config)) return;

            if (!string.IsNullOrEmpty(config.Name))
            {
                switch (args.ChangeType)
                {
                    case EntityChangeType.Created:
                    {
                        if (!Data.ContainsKey(config.Name))
                        {
                            Data.Add(config.Name, config.Value);
                        }

                        break;
                    }
                    case EntityChangeType.Updated:
                    {
                        Data.Remove(config.Name);
                        if (!config.IsDeleted)
                        {
                            Data.Add(config.Name, config.Value);
                        }

                        break;
                    }
                    case EntityChangeType.Deleted:
                        Data.Remove(config.Name);
                        break;
                }
            }
        }
    }
}