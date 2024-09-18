using Microsoft.Extensions.Hosting;

namespace VoteApp.SharedCommon
{
    public static class EnvironmentExtensions
    {
        public readonly static string ProductionEnvironmentName = "prod";
        public readonly static string EnvironmentVariableName = "ASPNETCORE_ENVIRONMENT";

        public static bool IsNotRestricted(this IHostEnvironment hostEnv)
        {
            return hostEnv.IsAnyQA() || hostEnv.IsLOCAL();
        }

        public static bool IsRestricted(this IHostEnvironment hostEnv)
        {
            return !hostEnv.IsNotRestricted();
        }

        public static bool IsPROD(this IHostEnvironment hostEnv)
        {
            return hostEnv.EnvironmentName == ProductionEnvironmentName;
        }

        public static bool IsAnyQA(this IHostEnvironment hostEnv)
        {
            return hostEnv.EnvironmentName.StartsWith("qa");
        }

        public static bool IsAnyUAT(this IHostEnvironment hostEnv)
        {
            return hostEnv.EnvironmentName.StartsWith("uat");
        }

        public static bool IsLOCAL(this IHostEnvironment hostEnv)
        {
            return hostEnv.EnvironmentName == "local";
        }
    }
}
