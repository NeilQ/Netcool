using System.ComponentModel.DataAnnotations;
using Netcool.Core.AppSettings;
using Netcool.Core.Services.Dto;

namespace Netcool.Api.Domain.Configuration
{
    public class AppConfigurationDto : EntityDto
    {
        public string Name { get; set; }

        public string Value { get; set; }

        public string Description { get; set; }

        public AppConfigurationType Type { get; set; }
    }

    public class AppConfigurationSaveInput : EntityDto
    {
        [Required(AllowEmptyStrings = false)]
        [MaxLength(256)]
        public string Name { get; set; }
        
        [Required(AllowEmptyStrings = false)]
        [MaxLength(256)]
        public string Value { get; set; }

        [MaxLength(256)]
        public string Description { get; set; }
    }
}