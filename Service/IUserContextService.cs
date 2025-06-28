using System.Security.Claims;

namespace RestaurationAPI.Service
{
    public interface IUserContextService
    {
        int? GetUserId { get; }
        ClaimsPrincipal User { get; }
    }
}