using Microsoft.AspNetCore.Mvc;
using Netcool.Api.Domain.Users;
using Netcool.Core.Services.Dto;
using Netcool.Core.WebApi.Controllers;

namespace Netcool.Api.Controllers
{
    [Route("users")]
    public class UsersController : CrudControllerBase<UserDto, int, PageRequest, UserSaveInput>
    {
        public UsersController(IUserService userService) :
            base(userService)
        {
        }
    }
}