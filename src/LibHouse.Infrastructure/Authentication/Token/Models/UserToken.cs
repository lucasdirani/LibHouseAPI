using System.Collections.Generic;

namespace LibHouse.Infrastructure.Authentication.Token.Models
{
    public class UserToken
    {
        public AuthenticatedUser User { get; }
        public string AccessToken { get; }
        public double ExpiresIn { get; }
        public IEnumerable<UserClaim> Claims { get; }

        public UserToken(
            AuthenticatedUser user, 
            string accessToken, 
            double expiresIn, 
            IEnumerable<UserClaim> claims)
        {
            User = user;
            AccessToken = accessToken;
            ExpiresIn = expiresIn;
            Claims = claims;
        }
    }
}