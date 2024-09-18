using Microsoft.AspNetCore.Http;
using Polly;
using Polly.Registry;
using Polly.Retry;
using Serilog;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.Common;
using System.Linq;
using System.Threading.Tasks;

namespace VoteApp.Api.StartupConfigs
{
    public class PollyMiddleware
    {
        public const string PollyPipelineKey = "polly-pipeline";
        public static ReadOnlyCollection<string> DbExceptionCodes => new ReadOnlyCollection<string>(new List<string> { "" });

        private readonly RequestDelegate _next;

        public PollyMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context, ResiliencePipelineProvider<string> pollyPipelineProvider)
        {
            var pipeline = pollyPipelineProvider.GetPipeline(PollyPipelineKey);

            await pipeline.ExecuteAsync(async token =>
            {
                await _next(context);
            });
        }

        public static RetryStrategyOptions PollyRetryStrategyOptionsFactory(AppConfig appConfig)
        {
            return new RetryStrategyOptions
            {
                ShouldHandle = new PredicateBuilder().Handle<DbException>(exception =>
                {
                    var isErrorFound = DbExceptionCodes.Any(dbErrorCode => exception.Message?.Contains(dbErrorCode) == true);

                    if (isErrorFound)
                    {
                        Log.Information("Polly caught db error: {Message}", exception.Message);
                    }

                    return isErrorFound;
                }),
                BackoffType = DelayBackoffType.Exponential,
                UseJitter = true,
                MaxRetryAttempts = appConfig.PollySettings.MaxRetryAttempts,
                Delay = appConfig.PollySettings.DelayBetweenRetries,
                OnRetry = (args) =>
                {
                    Log.Information("Polly retrying... attempt no: {AttemptNumber}", args.AttemptNumber + 1);
                    return default;
                }
            };
        }
    }
}
