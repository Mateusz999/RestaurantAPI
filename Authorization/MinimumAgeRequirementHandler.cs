﻿using Microsoft.AspNetCore.Authorization;
using System.Data;
using System.Security.Claims;

namespace RestaurationAPI.Authorization
{
    public class MinimumAgeRequirementHandler : AuthorizationHandler<MinimumAgeRequirement>
    {
        private readonly ILogger<MinimumAgeRequirementHandler> _logger;

        public MinimumAgeRequirementHandler(ILogger<MinimumAgeRequirementHandler> logger)
        {
            _logger = logger;
        }
        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, MinimumAgeRequirement requirement)
        {
           var dateOfBirth =  DateTime.Parse( context.User.FindFirst(c => c.Type == "DateOfBirth").Value);

            var userEmail = context.User.FindFirst(c => c.Type == ClaimTypes.Name).Value;

            _logger.LogInformation($"User: {userEmail} with date of birth: {dateOfBirth} ");

            if(dateOfBirth.AddYears(requirement.MinimumAge) < DateTime.Today)
            {
                _logger.LogInformation("Authorization Succeded");
                context.Succeed(requirement);
            }else
            {
                _logger.LogInformation("Authorization Failed");

            }

            return Task.CompletedTask;
        }
    }
}
