using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authorization.Infrastructure;
using Microsoft.Extensions.DependencyInjection;
using Netcool.Api.Domain.Users;
using Netcool.Core.Authorization;

namespace Netcool.Api.Domain.Authorization
{
    public class PermissionAuthorizationHandler : AuthorizationHandler<OperationAuthorizationRequirement>
    {
        private readonly IServiceScopeFactory _scopeFactory;

        public PermissionAuthorizationHandler(IServiceScopeFactory scopeFactory)
        {
            _scopeFactory = scopeFactory;
        }

        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context,
            OperationAuthorizationRequirement requirement)
        {
            var u = context.User;
            var idClaim = u.Claims.FirstOrDefault(x => x.Type == AppClaimTypes.UserId);

            using var scope = _scopeFactory.CreateScope();
            var userRepository = scope.ServiceProvider.GetRequiredService<IUserRepository>();

            // Do your stuff with TicketMasterRepository             
            if (!string.IsNullOrEmpty(idClaim?.Value) && int.TryParse(idClaim.Value, out var userId))
            {
                var permissions = userRepository.GetUserPermissions(userId);
                var permissionCodes = permissions
                    .Select(t => t.Code.ToLower())
                    .Distinct()
                    .ToList();
                if (permissionCodes.Contains(requirement.Name)) context.Succeed(requirement);
            }


            return Task.CompletedTask;
        }
    }
}