using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using VoteApp.Api.StartupConfigs;

namespace VoteApp.Api
{
    /*
     * [DESC]
     * Keeping the old-school Startup schema (removed in .NET 6) still does a good job :)
     */
    public class Startup
    {
        public AppConfig AppConfig { get; }
        public IWebHostEnvironment Environment { get; set; }

        public Startup(IWebHostEnvironment env)
        {
            var cfgBuilder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true)
                .AddEnvironmentVariables();

            AppConfig = new AppConfig(cfgBuilder.Build());
            Environment = env;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers()
                .AddNewtonsoftJson(options => options.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore);

            services.AddAppCors(Environment, AppConfig);

            services.AddAppAuthentication(AppConfig);

            services.AddMvc();

            services.AddHttpContextAccessor();

            services.AddVoteAppDI(AppConfig);

            services.AddVoteAppSwagger();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, ILoggerFactory loggerFactory)
        {
            var loggerForHttpRequests = loggerFactory.CreateLogger("HttpRequest");

            app.UseExceptionHandler("/api/error");

            app.UseStaticFiles();

            app.UseSwagger(c =>
            {
                c.SerializeAsV2 = true;
            });

            if (env.IsNotRestricted())
            {
                app.UseSwaggerUI(c =>
                {
                    c.SwaggerEndpoint("../swagger/v1/swagger.json", "VoteApp API");

                });
            }

            app.UseRouting();

            app.UseCors(CorsConfig.AppCorsPolicy);

            app.UseHttpsRedirection();

            app.UseVoteAppPollyMiddleware();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseLoggingHttpRequestsDataMiddleware(loggerForHttpRequests);

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
