using LibHouse.Business.Entities.Users;
using LibHouse.Infrastructure.Authentication.Token.Models;
using System;

namespace LibHouse.Infrastructure.Authentication.Login.Models
{
    public class AuthenticatedUser
    {
        public AuthenticatedUserProfile Profile { get; }
        public AccessToken AccessToken { get; }

        public AuthenticatedUser(
            AuthenticatedUserProfile profile,
            AccessToken accessToken)
        {
            Profile = profile;
            AccessToken = accessToken;
        }

        public override string ToString()
        {
            return $"Authenticated user {Profile.FullName}: {Profile.Id}";
        }
    }
}