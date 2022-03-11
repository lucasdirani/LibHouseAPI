using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.IO;

namespace LibHouse.Data.Context
{
    public class DesignTimeDbContextFactory : IDesignTimeDbContextFactory<LibHouseContext>
    {
        const string AppSettingsPath = "/../LibHouse.API/appsettings.json";

        public LibHouseContext CreateDbContext(string[] args)
        {
            var configuration = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory()).AddJsonFile(@Directory.GetCurrentDirectory() + AppSettingsPath).Build();

            var builder = new DbContextOptionsBuilder<LibHouseContext>();

            string connectionString = configuration.GetConnectionString("LibHouseConnectionString");

            builder
             .UseSqlServer(connectionString, s => s.CommandTimeout(180).EnableRetryOnFailure(3, TimeSpan.FromSeconds(5), null))
             .LogTo(Console.WriteLine, LogLevel.Information, DbContextLoggerOptions.LocalTime | DbContextLoggerOptions.SingleLine)
             .EnableDetailedErrors()
             .EnableSensitiveDataLogging();

            return new LibHouseContext(builder.Options);
        }
    }
}