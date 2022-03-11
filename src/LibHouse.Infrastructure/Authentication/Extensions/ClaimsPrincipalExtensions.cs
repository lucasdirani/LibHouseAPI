using System;
using System.Security.Claims;

namespace LibHouse.Infrastructure.Authentication.Extensions
{
    public static class ClaimsPrincipalExtensions
    {
        public static string GetUserId(this ClaimsPrincipal claimsPrincipal)
        {
            if (claimsPrincipal is null)
                throw new ArgumentException($"User claims were not found: {nameof(GetUserId)}");

            var claim = claimsPrincipal.FindFirst(ClaimTypes.NameIdentifier);

            return claim?.Value;
        }

        public static string GetUserEmail(this ClaimsPrincipal claimsPrincipal)
        {
            if (claimsPrincipal is null)
                throw new ArgumentException($"User claims were not found: {nameof(GetUserEmail)}");

            var claim = claimsPrincipal.FindFirst(ClaimTypes.Email);

            return claim?.Value;
        }

        public static string GetUserName(this ClaimsPrincipal claimsPrincipal)
        {
            if (claimsPrincipal is null)
                throw new ArgumentException($"User claims were not found: {nameof(GetUserName)}");

            var claim = claimsPrincipal.FindFirst(ClaimTypes.Name);

            return claim?.Value;
        }
    }
}