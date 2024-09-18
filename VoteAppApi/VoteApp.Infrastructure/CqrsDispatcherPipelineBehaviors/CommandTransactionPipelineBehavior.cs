using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System.Threading;
using System.Threading.Tasks;
using VoteApp.Application;
using VoteApp.Domain.Exceptions;

namespace VoteApp.Infrastructure.Database
{
    /*
     * [DESC]
     * We are using MediatR's PipelineBehavior to ensure database transactions at the handler level.
     */
    public class CommandTransactionPipelineBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
        where TRequest : class, IRequest<TResponse>
    {
        private readonly ILogger _logger;
        private readonly ICurrentUser _currentUser;
        private readonly IUnitOfWork _uow;
        private readonly IRequestHandler<TRequest, TResponse> _requestHandler;
        private readonly IHandlerContext _handlerContext;
        private readonly ApplicationContext _applicationContext;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public CommandTransactionPipelineBehavior(ILogger<CommandTransactionPipelineBehavior<TRequest, TResponse>> logger,
            ICurrentUser currentUser, IUnitOfWork uow,
            IRequestHandler<TRequest, TResponse> requestHandler,
            IHandlerContext handlerContext,
            ApplicationContext applicationContext,
            IHttpContextAccessor httpContextAccessor)
        {
            _logger = logger;
            _currentUser = currentUser;
            _uow = uow;
            _requestHandler = requestHandler;
            _handlerContext = handlerContext;
            _applicationContext = applicationContext;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<TResponse> Handle(TRequest request, CancellationToken cancellationToken, RequestHandlerDelegate<TResponse> next)
        {
            var (executionContextId, handlerName, currentUser, inputData) = FetchLoggingData(request);

            using (_logger.BeginScope(LoggingHelper.GetScopedDictionary(executionContextId, handlerName, currentUser, inputData)))
            {
                _httpContextAccessor.HttpContext.SetScopedLoggingData(handlerName, currentUser, inputData);

                var logMessage = $"Executing handler: | {LoggingHelper.ScopedLogMessageForHandler}";
                _logger.LogInformation(logMessage);

                if (!string.IsNullOrWhiteSpace(_handlerContext.LockId))
                {
                    /*
                     * [DESC]
                     * There are situations where we want to ensure the single usage of a given handler at the same time.
                     * This lock mechanism can achieve that, but we need to carefully analyze the functionality case, as it works in a pessimistic way.
                     * In some sense the lock mechnism plays role of quequing mechnism.
                     * This solution is protected against potential problems by using a timeout.
                     */
                    await _applicationContext.WaitLock(_handlerContext.LockId);

                    try
                    {
                        return await ExecuteWithAspectOptions(next);
                    }
                    finally
                    {
                        _applicationContext.ReleaseLock(_handlerContext.LockId);
                    }
                }
                else
                {
                    return await ExecuteWithAspectOptions(next);
                }
            }
        }

        private (string executionContextId, string handlerName, string currentUser, string inputData) FetchLoggingData(TRequest request)
        {
            var handlerName = (_requestHandler.GetType())?.Name ?? "N/A";
            var currentUser = _currentUser.IsAuthenticated ? _currentUser.UserId! : "N/A";
            var inputData = request.SerializeToJsonForLog();
            var executionContextId = _httpContextAccessor?.HttpContext?.TraceIdentifier ?? "N/A";

            return (executionContextId, handlerName, currentUser, inputData);
        }

        private async Task<TResponse> ExecuteWithAspectOptions(RequestHandlerDelegate<TResponse> next)
        {
            TResponse response;

            using (var trScope = _uow.CreateTransactionScope())
            {
                try
                {
                    response = await next();
                }
                catch (UserException)
                {
                    // [DESC] Sometimes, there can be functionality that requires saving some data despite a user error occurring.
                    if (_handlerContext.IsForceCompleteTransaction == true)
                    {
                        await _uow.CommitAsync();
                        trScope.Complete();
                    }

                    throw;
                }

                await _uow.CommitAsync();
                trScope.Complete();
            }

            await ExecuteActionsAfterDbCommitSuccess();

            return response;
        }

        private static Task ExecuteActionsAfterDbCommitSuccess()
        {
            // Here run actions (needs to be implemeneted) 
            return Task.CompletedTask;
        }
    }
}
