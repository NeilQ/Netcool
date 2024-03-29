﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Netcool.Api.Domain.Users;
using Netcool.Core.AspNetCore.Controllers;
using Netcool.Core.Services.Dto;

namespace Netcool.Api.Controllers;

[Route("user-login-attempts")]
[Authorize]
public class UserLoginAttemptsController : QueryControllerBase<UserLoginAttemptDto, int, PageRequest>
{
    public UserLoginAttemptsController(IUserLoginAttemptService service) : base(service)
    {
    }
}