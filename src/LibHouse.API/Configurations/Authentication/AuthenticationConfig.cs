using LibHouse.Infrastructure.Authentication.Register;
using LibHouse.Infrastructure.Authentication.Token.Generators;
using LibHouse.Infrastructure.Authentication.Token.Settings;
using LibHouse.Infrastructure.Authentication.Token.Validations;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace LibHouse.API.Configurations.Authentication
{
    public static class AuthenticationConfig
    {
        public static IServiceCollection AddAuthenticationConfiguration(
            this IServiceCollection services,
            IConfiguration configuration)
        {
            var tokenSettingsSection = configuration.GetSection("TokenSettings");
            services.Configure<TokenSettings>(tokenSettingsSection);

            var tokenSettings = tokenSettingsSection.Get<TokenSettings>();
            var key = Encoding.ASCII.GetBytes(tokenSettings.Secret);
            var tokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(key),
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = true,
                ValidAudience = tokenSettings.ValidIn,
                ValidIssuer = tokenSettings.Issuer,
            };

            services.AddSingleton(tokenValidationParameters);

            services.AddAuthentication(x => {
                x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(x =>
            {
                x.RequireHttpsMetadata = true;
                x.SaveToken = true;
                x.TokenValidationParameters = tokenValidationParameters;
            });

            services.AddScoped<ITokenGenerator, JwtTokenGenerator>();

            services.AddSingleton<IRefreshTokenValidator, RefreshTokenValidator>();

            services.AddScoped<IUserSignUp, IdentityUserSignUp>();

            services.AddScoped<IUserSignIn, IdentityUserSignIn>();

            return services;
        }
    }
}