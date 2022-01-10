using Microsoft.AspNetCore.Authorization;

namespace RestaurantAPI.Authorization
{
    public class MinimumRestaurantsCreatedRequirement : IAuthorizationRequirement
    {
        public int MinimumRestaurantCreated { get; }

        public MinimumRestaurantsCreatedRequirement(int minimumRestaurantCreated)
        {
            MinimumRestaurantCreated = minimumRestaurantCreated;
        }
    }
}
