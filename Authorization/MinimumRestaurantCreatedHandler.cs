using Microsoft.AspNetCore.Authorization;
using RestaurationAPI.Entities;
using System.Security.Claims;

namespace RestaurationAPI.Authorization
{
    
    public class MinimumRestaurantCreatedHandler : AuthorizationHandler<MinimumRestaurantCreated>
    {
        private readonly RestaurantDBContext _cnt;
        public MinimumRestaurantCreatedHandler(RestaurantDBContext cnt)
        {
            _cnt = cnt;
        }


        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, MinimumRestaurantCreated requirement)
        {
            int userId = int.Parse(context.User.FindFirst(c => c.Type == ClaimTypes.NameIdentifier).Value);
            int restaurationCreatedCounter = _cnt
                .Restaurants
                .Count(r => r.CreatedById == userId);

            if(restaurationCreatedCounter >= requirement.MinimumRestaurant)
            {
                context.Succeed(requirement);
            }

            return Task.CompletedTask;
        }
    }
}
