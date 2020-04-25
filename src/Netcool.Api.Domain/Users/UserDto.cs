using System.ComponentModel.DataAnnotations;
using Netcool.Core.Helpers;
using Netcool.Core.Services.Dto;

namespace Netcool.Api.Domain.Users
{
    public class UserDto : UserSaveInput
    {
        public string GenderDescription => Reflection.GetEnumDescription(Gender);
    }

    public class UserSaveInput : EntityDto
    {
        [Required(AllowEmptyStrings = false)]
        [MaxLength(64)]
        public string Name { get; set; }

        [MaxLength(64)]
        public string DisplayName { get; set; }

        public Gender Gender { get; set; }

        [MaxLength(256)]
        public string Email { get; set; }

        [MaxLength(64)]
        public string Phone { get; set; }

        /// <summary>
        /// 是否启用
        /// </summary>
        public bool IsActive { get; set; }
    }
}