using System;

namespace VoteApp.SharedCommon
{
    /// <summary>
    /// Reassigning (SetDateTime) allowed in unit tests mode only! For tests based on current date this provider helps taking control over setting current date for Now.
    /// Running unit tests in parallel mode not recommended for logic using the provider.
    /// </summary>
    public static class DateTimeProvider
    {
        private static Func<DateTime> _now = () => DateTime.Now;
        private static readonly bool _isProductionEnvironment = Environment.GetEnvironmentVariable(EnvironmentExtensions.EnvironmentVariableName) == EnvironmentExtensions.ProductionEnvironmentName;

        public static DateTime Now => _now();

#if DEBUG
        public static void SetDateTime(DateTime dateTime)
        {
            if (_isProductionEnvironment)
            {
                throw new InvalidOperationException("Cannot set custom date/time outside of a test environment.");
            }

            _now = () => dateTime;
        }

        public static void ResetToDefault()
        {
            if (_isProductionEnvironment)
            {
                throw new InvalidOperationException("Cannot reset custom date/time outside of a test environment.");
            }

            _now = () => DateTime.Now;
        }
#endif
    }

}
