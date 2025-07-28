using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.DependencyInjection;
using Service.interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.Services.Auth
{
    public class PermissionAuthorizationHandler : AuthorizationHandler<PermissionRequirement>
    {
        private readonly IUserService _userService;
        public PermissionAuthorizationHandler(IUserService userService)
        {
            _userService = userService;
        }
        protected override async Task HandleRequirementAsync(AuthorizationHandlerContext context, PermissionRequirement requirement)
        {
            var userId = context.User.Claims.FirstOrDefault(x => x.Type == "userId");
            if (userId == null || !int.TryParse(userId.Value, out var id))
            {
                return;
            }
            var permissions = await _userService.GetUserPermission(id);

            if (permissions.Intersect(requirement.Permissions).Any())
            {
                context.Succeed(requirement);
            }
        }
    }
}
