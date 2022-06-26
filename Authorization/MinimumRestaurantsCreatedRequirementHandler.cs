using Microsoft.AspNetCore.Authorization;
using RestaurantAPI.Entities;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace RestaurantAPI.Authorization
{
    public class MinimumRestaurantsCreatedRequirementHandler : AuthorizationHandler<MinimumRestaurantsCreatedRequirement>
    {
        private readonly RestaurantDbContext _dbContext;
        public MinimumRestaurantsCreatedRequirementHandler(RestaurantDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, MinimumRestaurantsCreatedRequirement requirement)
        {
            var userId = int.Parse(context.User.FindFirst(c => c.Type == ClaimTypes.NameIdentifier).Value);

            var restaurantsCreatedCount = _dbContext
               .Restaurants
               .Where(x => x.CreatedById == userId)
               .Count();

            if (restaurantsCreatedCount >= requirement.MinimumRestaurantCreated)
            {
                context.Succeed(requirement);
            }

            return Task.CompletedTask;
        }
    }
}
