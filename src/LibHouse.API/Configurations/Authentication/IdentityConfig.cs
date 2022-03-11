using LibHouse.Infrastructure.Authentication.Context;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace LibHouse.API.Configurations.Authentication
{
    public static class IdentityConfig
    {
        public static IServiceCollection AddIdentityConfiguration(
            this IServiceCollection services,
            IConfiguration configuration)
        {
            string connectionString = configuration.GetConnectionString("LibHouseAuthConnectionString");

            services.AddDbContext<AuthenticationContext>(options => options.UseSqlServer(connectionString));

            services
              .AddIdentity<IdentityUser, IdentityRole>()
              .AddEntityFrameworkStores<AuthenticationContext>()
              .AddDefaultTokenProviders();

            return services;
        }
    }
}