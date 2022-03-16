using AutoMapper;
using LibHouse.API.V1.Profiles.Users;
using Microsoft.Extensions.DependencyInjection;
using System.Collections.Generic;

namespace LibHouse.API.Configurations.Mapping
{
    public static class AutoMapperConfig
    {
        public static IServiceCollection AddAutoMapperConfigurationForV1(this IServiceCollection services)
        {
            services.AddAutoMapper(config =>
            {
                config.AddProfiles(new List<Profile>
                {
                    new UserProfile(),
                });
            });

            return services;
        }
    }
}