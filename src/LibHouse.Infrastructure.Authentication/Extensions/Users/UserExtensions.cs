using LibHouse.Business.Entities.Users;
using LibHouse.Infrastructure.Authentication.Roles;
using Microsoft.AspNetCore.Identity;

namespace LibHouse.Infrastructure.Authentication.Extensions.Users
{
    internal static class UserExtensions
    {
        public static IdentityUser AsNewIdentityUser(this User user)
        {
            return new()
            {
                EmailConfirmed = false,
                Email = user.Email,
                UserName = user.Email,
                PhoneNumber = user.Phone,
            };
        }

        public static string GetUserTypeRole(this User user)
        {
            return user.UserType is UserType.Resident 
                ? LibHouseUserRole.Resident 
                : LibHouseUserRole.Owner;
        }
    }
}