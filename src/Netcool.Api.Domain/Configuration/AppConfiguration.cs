using Netcool.Core.AppSettings;
using Netcool.Core.Entities;

namespace Netcool.Api.Domain.Configuration
{
    public class AppConfiguration : FullAuditEntity
    {
        public string Name { get; set; }

        public string Value { get; set; }

        public string Description { get; set; }

        public AppConfigurationType Type { get; set; }
    }
}