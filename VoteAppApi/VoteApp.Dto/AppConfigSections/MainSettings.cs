using System;

namespace VoteApp.Dto.AppConfigSections
{
    public class MainSettings
    {
        public TimeSpan DbTransactionTimeout { get; set; }
        public string ClientAddress { get; set; } = string.Empty;
    }
}
