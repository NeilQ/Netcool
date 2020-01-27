using System.ComponentModel.DataAnnotations;
using Netcool.Core.Services.Dto;

namespace Netcool.Api.Domain.Users
{
    public class UserDto : UserSaveInput
    {
        public bool IsRoot { get; set; }
    }

    public class UserSaveInput : EntityDto
    {
        [MaxLength(64)]
        public string Name { get; set; }

        [MaxLength(64)]
        public string DisplayName { get; set; }

        public Gender Gender { get; set; }

        [MaxLength(64)]
        public string Email { get; set; }

        [MaxLength(16)]
        public string Phone { get; set; }

        public bool IsActive { get; set; }
    }
}