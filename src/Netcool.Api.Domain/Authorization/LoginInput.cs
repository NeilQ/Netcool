using System.ComponentModel.DataAnnotations;

namespace Netcool.Api.Domain.Authorization
{
    public class LoginInput
    {
        [Required(AllowEmptyStrings = false)]
        [MaxLength(64)]
        public string Name { get; set; }
        [Required(AllowEmptyStrings = false)]
        [MinLength(5)]
        public string Password { get; set; }
    }
}