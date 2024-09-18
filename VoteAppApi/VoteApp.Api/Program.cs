using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Serilog;
using Serilog.Filters;
using System;
using VoteApp.Api.StartupConfigs;

namespace VoteApp.Api
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var config = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .Build();

            Log.Logger = new LoggerConfiguration()
               .Enrich.FromLogContext()
               .ReadFrom.Configuration(config)
               .Filter.ByExcluding(Matching.FromSource("Microsoft"))
               .CreateBootstrapLogger();

            try
            {
                var webAppBuilder = WebApplication.CreateBuilder(args);
                webAppBuilder.Host.UseSerilog((hostContext, services, loggerConfig) =>
                {
                    loggerConfig
                    .ReadFrom.Services(services)
                    .ReadFrom.Configuration(config)
                    .Enrich.FromLogContext()
                    .Filter.ByExcluding(Matching.FromSource("Microsoft"));
                });


                Log.Information("Starting up: {ApplicationName} | {EnvironmentName}", webAppBuilder.Environment.ApplicationName, webAppBuilder.Environment.EnvironmentName);

                var startup = new Startup(webAppBuilder.Environment);

                startup.ConfigureServices(webAppBuilder.Services);

                var webApp = webAppBuilder.Build();

                startup.Configure(webApp, webApp.Environment, webApp.Services.GetRequiredService<ILoggerFactory>());

                Log.Information("Executing DB migrations");

                webApp.ExecuteDbMigrations();

                Log.Information("Running");

                webApp.Run();
            }
            catch (Exception ex)
            {
                Log.Fatal(ex, "Application start-up failed");
            }
            finally
            {
                Log.Information("Exited");
                Log.CloseAndFlush();
            }
        }
    }

}



