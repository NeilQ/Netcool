using Netcool.Core.Services.Dto;

namespace Netcool.Api.Domain.Users
{
    public class UserRequest : PageRequest
    {
        public string Name { get; set; }

        public string Phone { get; set; }

        public string Email { get; set; }

        public Gender? Gender { get; set; }

        public int? OrganizationId { get; set; }
    }
}