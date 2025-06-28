using RestaurationAPI.Entities;
using System.Security.Claims;

namespace RestaurationAPI.Service
{
    public class UserContextService : IUserContextService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public UserContextService(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }


        public ClaimsPrincipal User => _httpContextAccessor.HttpContext?.User;


        public int? GetUserId => User is null ? null : (int?)int.Parse(User.FindFirst(C => C.Type == ClaimTypes.NameIdentifier).Value);

    }
}
