using VoteApp.Dto.AppConfigSections;

namespace VoteApp.Application
{
    public interface IAppConfig
    {
        MainSettings MainSettings { get; }
        PollySettings PollySettings { get; }
    }
}
