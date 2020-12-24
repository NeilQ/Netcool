using Netcool.Core.Services.Dto;

namespace Netcool.Api.Domain.Configuration
{
    public class AppConfigurationRequest: PageRequest
    {
        public string Name { get; set; }
    }
}