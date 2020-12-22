using Netcool.Core.AppSettings;
using Netcool.Core.Entities;

namespace Netcool.Api.Domain.Configuration
{
    public class AppConfiguration : FullAuditEntity
    {
        public AppConfiguration()
        {
        }

        public AppConfiguration(int id, string name, string value, string description, AppConfigurationType type,
            bool isInitial)
        {
            Id = id;
            Name = name;
            Value = value;
            Description = description;
            Type = type;
            IsInitial = isInitial;
        }

        public string Name { get; set; }

        public string Value { get; set; }

        public string Description { get; set; }

        public AppConfigurationType Type { get; set; }

        /// <summary>
        /// Initial config cannot be deleted.
        /// </summary>
        public bool IsInitial { get; set; }
    }
}