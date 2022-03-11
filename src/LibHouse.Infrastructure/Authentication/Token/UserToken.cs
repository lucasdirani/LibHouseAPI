using LibHouse.Business.Entities.Users;
using System.Collections.Generic;

namespace LibHouse.Infrastructure.Authentication.Token
{
    public class UserToken
    {
        public User User { get; }
        public string AccessToken { get; }
        public double ExpiresIn { get; }
        public IEnumerable<UserClaim> Claims { get; }

        public UserToken(
            User user, 
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