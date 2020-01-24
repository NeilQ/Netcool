using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Netcool.Core.Entities;

namespace Netcool.Api.Domain.Users
{
    public class User : FullAuditEntity
    {
        public string Name { get; set; }

        public string DisplayName { get; set; }

        public string Password { get; set; }

        public Gender Gender { get; set; }

        public string Email { get; set; }

        public string Phone { get; set; }

        [DefaultValue(true)]
        public bool IsActive { get; set; }

        [DefaultValue(false)]
        public bool IsRoot { get; set; }
    }
}