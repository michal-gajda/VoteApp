using Microsoft.Extensions.Configuration;
using VoteApp.Api.StartupConfigs;
using VoteApp.Application;
using VoteApp.Dto.AppConfigSections;

namespace VoteApp.Api
{
    public class AppConfig : IAppConfig
    {
        private readonly IConfiguration _configuration;
        public IConfiguration Configuration => _configuration;

        public AppConfig(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public string VoteAppConnectionString => _configuration.GetConnectionString("VOTEAPP");

        public MainSettings MainSettings => _configuration.GetTypedSection<MainSettings>();
        public PollySettings PollySettings => _configuration.GetTypedSection<PollySettings>();
    }
}
