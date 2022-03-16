using LibHouse.Infrastructure.Email;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace LibHouse.API.Configurations.Email
{
    internal static class EmailConfig
    {
        public static IServiceCollection AddEmailConfig(
            this IServiceCollection services,
            IConfiguration configuration)
        {
            services.Configure<MailSettings>(options => configuration.GetSection("MailSettings").Bind(options));

            services.AddSingleton<IMailService, MailKitService>();

            return services;
        }
    }
}