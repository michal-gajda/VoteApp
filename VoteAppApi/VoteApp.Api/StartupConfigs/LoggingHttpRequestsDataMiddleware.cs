using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System;
using System.Diagnostics;
using System.Threading.Tasks;

namespace VoteApp.Api.StartupConfigs
{
    public class LoggingHttpRequestsDataMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger _logger;

        public LoggingHttpRequestsDataMiddleware(RequestDelegate next, ILogger logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            var (executionContextId, handlerName, currentUser, inputData) = await LoggingHelper.FetchLoggingDataFromRequest(context);

            using (_logger.BeginScope(LoggingHelper.GetScopedDictionary(executionContextId, handlerName, currentUser, inputData)))
            {
                PreActionLogging(context, handlerName, currentUser, inputData);

                var timer = new Stopwatch();
                timer.Start();

                await _next(context!);

                timer.Stop();

                PostActionLoggin(handlerName, timer);
            }
        }

        private void PostActionLoggin(string handlerName, Stopwatch timer)
        {
            try
            {
                var minutes = Math.Round(timer.Elapsed.TotalMinutes, 1);
                var seconds = Math.Round(timer.Elapsed.TotalSeconds, 0);

                if (!handlerName.StartsWith("/api/error")) // ignore logging for ErrorController
                {
                    var logMessage = "Executed HTTP request successfully ";
                    if (seconds > 0 && minutes < 1)
                    {
                        logMessage += $"{seconds} sec";
                    }
                    else if (minutes >= 1)
                    {
                        logMessage += $"{minutes} mins ({seconds} sec)";
                    }

                    logMessage += $" | {LoggingHelper.ScopedLogMessageForRequest}";

                    _logger?.LogInformation(logMessage);
                }
            }
            catch (Exception ex)
            {
                _logger?.LogInformation(ex, "Error during metrics: {Message}", ex.Message);
            }
        }

        private void PreActionLogging(HttpContext context, string handlerName, string currentUser, string inputData)
        {
            if (!handlerName.StartsWith("/api/error")) // ignore logging for ErrorController
            {
                context.SetScopedLoggingData(handlerName, currentUser, inputData);
                var logMessage = $"Executing HTTP request: | {LoggingHelper.ScopedLogMessageForRequest}";
                _logger.LogInformation(logMessage);
            }
        }
    }
}
