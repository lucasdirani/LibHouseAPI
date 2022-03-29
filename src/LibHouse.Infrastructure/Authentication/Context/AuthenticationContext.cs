using LibHouse.Infrastructure.Authentication.Context.Configurations;
using LibHouse.Infrastructure.Authentication.Token.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace LibHouse.Infrastructure.Authentication.Context
{
    public class AuthenticationContext : IdentityDbContext
    {
        public DbSet<RefreshToken> RefreshTokens { get; set; }

        public AuthenticationContext(DbContextOptions<AuthenticationContext> options) 
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.ApplyConfiguration(new RefreshTokenConfiguration());

            builder.ApplyConfiguration(new IdentityRoleConfiguration());

            base.OnModelCreating(builder);
        }
    }
}