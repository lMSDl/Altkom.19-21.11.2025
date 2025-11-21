using Microsoft.AspNetCore.Authorization;
using Models;

namespace UsersWebApp.Policy.Handlers
{
    public class MinimumAgeHandler : AuthorizationHandler<MinimumAgePolicy>
    {
        /*public MinimumAgeHandler(ILogger<MinimumAgeHandler> logger)
        {
        }*/

        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, MinimumAgePolicy requirement)
        {
            if (context.User.HasClaim(x => x.Type == nameof(User.Age)))
            {
                var ageClaim = context.User.FindFirst(x => x.Type == nameof(User.Age));
                if(int.Parse(ageClaim.Value) >= 25)
                    context.Succeed(requirement);
            }
            return Task.CompletedTask;
        }
    }
}
