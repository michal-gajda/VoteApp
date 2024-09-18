using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using VoteApp.Infrastructure.Database;

namespace VoteApp.Api.StartupConfigs
{
    public static class ConfigExtensions
    {
        public static IApplicationBuilder UseLoggingHttpRequestsDataMiddleware(this IApplicationBuilder builder, ILogger loggerForHttpRequests)
        {
            return builder.UseMiddleware<LoggingHttpRequestsDataMiddleware>(loggerForHttpRequests);
        }

        public static IApplicationBuilder UseVoteAppPollyMiddleware(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<PollyMiddleware>();
        }

        public static void ExecuteDbMigrations(this IHost host)
        {
            var context = host.Services.GetRequiredService<VoteAppDbContext>();

            if (context.Database.ProviderName != "Microsoft.EntityFrameworkCore.InMemory")
            {
                // For the sample Voting app on GitHub to be more convenient, it automatically creates the database; normally, EF migrations are used here.
                context.Database.EnsureCreated();
            }
            VoteAppDbContext.Initialize(context);
        }

        public static T GetTypedSection<T>(this IConfiguration config)
        {
            return config.GetSection(typeof(T).Name).Get<T>();
        }
    }
}
