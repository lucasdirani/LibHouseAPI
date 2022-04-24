using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using System.IO;

namespace LibHouse.Infrastructure.Authentication.Context.Factories
{
    public class DesignTimeDbContextFactory : IDesignTimeDbContextFactory<AuthenticationContext>
    {
        const string AppSettingsPath = "/../LibHouse.API/appsettings.json";

        public AuthenticationContext CreateDbContext(string[] args)
        {
            var configuration = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory()).AddJsonFile(@Directory.GetCurrentDirectory() + AppSettingsPath).Build();
            
            var builder = new DbContextOptionsBuilder<AuthenticationContext>();
            
            string connectionString = configuration.GetConnectionString("LibHouseAuthConnectionString");
            
            builder.UseSqlServer(connectionString);

            return new AuthenticationContext(builder.Options);
        }
    }
}