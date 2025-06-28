using Microsoft.AspNetCore.Authorization;

namespace RestaurationAPI.Authorization
{
    public class MinimumRestaurantCreated : IAuthorizationRequirement
    {
        public int MinimumRestaurant { get; }

        public MinimumRestaurantCreated(int minimumRestaurat)
        {
            MinimumRestaurant = minimumRestaurat;
        }
    }
}
