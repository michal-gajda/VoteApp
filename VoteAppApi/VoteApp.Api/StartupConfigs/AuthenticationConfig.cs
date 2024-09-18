using Microsoft.Extensions.DependencyInjection;
namespace VoteApp.Api.StartupConfigs
{
    public static class AuthenticationConfig
    {
        public static void AddAppAuthentication(this IServiceCollection services, AppConfig appConfig)
        {
            // here put configurations responsible for authentications
        }
    }
}
