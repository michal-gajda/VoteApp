using Microsoft.Extensions.Configuration;
using Moq;
using VoteApp.Application;

namespace VoteApp.Tests.Common
{
    public static class OtherHelpers
    {
        public static ICurrentUser MockCurrentUser(this BaseTestContainer baseCtr, string userId, bool isAuthenticated = true)
        {
            var mock = new Mock<ICurrentUser>();

            mock.SetupGet(m => m.UserId).Returns(userId);
            mock.SetupGet(m => m.IsAuthenticated).Returns(isAuthenticated);

            return mock.Object;
        }

        public static IConfiguration CreateConfiguration(this BaseTestContainer baseCtr, Dictionary<string, string> settings = null)
        {
            IConfigurationBuilder builder = new ConfigurationBuilder();
            if (settings == null)
            {
                settings = new()
                {
                    { "MainSettings:DbTransactionTimeout", "00:01:00" }
                };
            }
            builder = builder.AddInMemoryCollection(settings);

            return builder.Build();
        }
    }
}
