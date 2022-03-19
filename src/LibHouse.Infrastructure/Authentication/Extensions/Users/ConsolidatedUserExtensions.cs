using LibHouse.Business.Projections.Users;
using LibHouse.Infrastructure.Authentication.Token.Models;

namespace LibHouse.Infrastructure.Authentication.Extensions.Users
{
    internal static class ConsolidatedUserExtensions
    {
        public static AuthenticatedUser AsAuthenticatedUser(
            this ConsolidatedUser consolidatedUser)
        {
            return new(
                consolidatedUser.Id, 
                consolidatedUser.FullName, 
                consolidatedUser.BirthDate, 
                consolidatedUser.Gender.ToString(), 
                consolidatedUser.Email, 
                consolidatedUser.UserType.ToString()
            );
        }
    }
}