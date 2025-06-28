using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.DataProtection.AuthenticatedEncryption;

namespace RestaurationAPI.Authorization
{
    public class MinimumAgeRequirement : IAuthorizationRequirement
    {
        public int MinimumAge { get; }


        public MinimumAgeRequirement(int miniumAge)
        {
            MinimumAge = MinimumAge;
        }
    }
}
