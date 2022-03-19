using LibHouse.API.Authentication;
using LibHouse.API.Configurations.Swagger;
using LibHouse.Business.Entities.Shared;
using LibHouse.Business.Entities.Users;
using LibHouse.Business.Notifiers;
using LibHouse.Business.Services.Users;
using LibHouse.Business.Validations.Users;
using LibHouse.Data.Context;
using LibHouse.Data.Repositories.Users;
using LibHouse.Data.Transactions;
using LibHouse.Infrastructure.Authentication.Token.Login;
using LibHouse.Infrastructure.Shared.Settings;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Swashbuckle.AspNetCore.SwaggerGen;
using System;

namespace LibHouse.API.Configurations.Dependencies
{
    public static class DependencyInjectionConfig
    {
        public static IServiceCollection ResolveGeneralDependencies(
            this IServiceCollection services,
            IConfiguration configuration)
        {
            services.Configure<LibHouseWebsiteSettings>(options 
                => configuration.GetSection("LibHouseWebsiteSettings").Bind(options));

            services.AddScoped<INotifier, Notifier>();

            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

            services.AddScoped<ILoggedUser, AspNetLoggedUser>();

            services.AddTransient<IConfigureOptions<SwaggerGenOptions>, ConfigureSwaggerOptions>();

            return services;
        }

        public static IServiceCollection ResolveValidators(this IServiceCollection services)
        {
            services.AddScoped<UserRegistrationValidator>();

            return services;
        }

        public static IServiceCollection ResolveServices(this IServiceCollection services)
        {
            services.AddScoped<IUserRegistrationService, UserRegistrationService>();

            return services;
        }

        public static IServiceCollection ResolveRepositories(
            this IServiceCollection services, 
            IConfiguration configuration)
        {
            string connectionString = configuration.GetConnectionString("LibHouseConnectionString");

            services.AddDbContext<LibHouseContext>(options => 
                options.UseSqlServer(
                    connectionString,
                    s => s.CommandTimeout(180).EnableRetryOnFailure(3, TimeSpan.FromSeconds(5), null)
                ).LogTo(
                    Console.WriteLine,
                    LogLevel.Information,
                    DbContextLoggerOptions.LocalTime | DbContextLoggerOptions.SingleLine
                ).EnableDetailedErrors()
                .EnableSensitiveDataLogging()
            );

            services.AddScoped<IUnitOfWork, UnitOfWork>();

            services.AddScoped<IUserRepository, UserRepository>();

            return services;
        }
    }
}