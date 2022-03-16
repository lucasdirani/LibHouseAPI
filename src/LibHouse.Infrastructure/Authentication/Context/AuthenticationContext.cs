using LibHouse.Infrastructure.Authentication.Roles;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace LibHouse.Infrastructure.Authentication.Context
{
    public class AuthenticationContext : IdentityDbContext
    {
        public AuthenticationContext(DbContextOptions<AuthenticationContext> options) 
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<IdentityRole>().HasData(new IdentityRole[]
            {
                new IdentityRole(LibHouseUserRole.User) 
                { 
                    NormalizedName = LibHouseUserRole.User.ToUpper() 
                },
                new IdentityRole(LibHouseUserRole.Resident)
                {
                    NormalizedName = LibHouseUserRole.Resident.ToUpper()
                },
                new IdentityRole(LibHouseUserRole.Owner)
                {
                    NormalizedName = LibHouseUserRole.Owner.ToUpper()
                },
            });

            base.OnModelCreating(builder);
        }
    }
}