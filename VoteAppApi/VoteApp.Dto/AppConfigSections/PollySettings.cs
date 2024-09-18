using System;

namespace VoteApp.Dto.AppConfigSections
{
    public class PollySettings
    {
        public int MaxRetryAttempts { get; set; } = 1;
        public TimeSpan DelayBetweenRetries { get; set; } = TimeSpan.FromSeconds(2);
    }
}
