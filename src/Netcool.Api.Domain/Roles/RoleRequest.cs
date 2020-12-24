using Netcool.Core.Services.Dto;

namespace Netcool.Api.Domain.Roles
{
    public class RoleRequest : PageRequest
    {
        public string Name { get; set; }
    }
}