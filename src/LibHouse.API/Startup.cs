using LibHouse.API.Configurations.Authentication;
using LibHouse.API.Configurations.Cache;
using LibHouse.API.Configurations.Core;
using LibHouse.API.Configurations.Dependencies;
using LibHouse.API.Configurations.Logging;
using LibHouse.API.Configurations.Swagger;
using LibHouse.API.Middlewares;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace LibHouse.API
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddApiResponseCompressionConfig();

            services.ResolveGeneralDependencies();

            services.ResolveRepositories(Configuration);

            services.AddIdentityConfiguration(Configuration);

            services.AddAuthenticationConfiguration(Configuration);

            services.AddApiCachingConfig(Configuration);

            services.AddWebApiConfig();

            services.AddLoggingConfiguration();

            services.AddSwaggerConfig();
        }

        public void Configure(
            IApplicationBuilder app, 
            IWebHostEnvironment env,
            IApiVersionDescriptionProvider provider)
        {
            app.UseResponseCompression();

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();

                app.UseCors("Development");
            }

            app.UseForwardedHeaders(new ForwardedHeadersOptions
            {
                ForwardedHeaders = ForwardedHeaders.XForwardedFor
                | ForwardedHeaders.XForwardedProto,
            });

            app.UseMiddleware<ExceptionMiddleware>();

            app.UseLoggingConfiguration(Configuration);

            app.UseMvcConfiguration();

            app.UseSwaggerConfig(provider);
        }
    }
}