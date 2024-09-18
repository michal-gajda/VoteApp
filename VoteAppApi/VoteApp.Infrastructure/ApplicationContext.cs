using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using VoteApp.Domain;
using VoteApp.Domain.Exceptions;

namespace VoteApp.Infrastructure
{
    public class ApplicationContext
    {
        // <lockId, (semaphor, timeout)>
        private readonly Dictionary<string, (SemaphoreSlim Semaphor, int Timeout)> _semaphors = new();
        private readonly ILogger<ApplicationContext> _logger;

        public ApplicationContext(ILogger<ApplicationContext> logger)
        {
            // registering timeouts for handlers' locks (can be moved to app settings)

#pragma warning disable S125 // Sections of code should not be commented out
                            //_semaphors.Add(CommandHandler.LockId, (new SemaphoreSlim(1), 15000));
#pragma warning restore S125 // Sections of code should not be commented out

            _logger = logger;
        }

        public async Task<bool> WaitLock(string lockId, bool isExceptionOnTimeout = true)
        {
            var lockData = _semaphors[lockId];

            var isTimeout = !(await lockData.Semaphor.WaitAsync(lockData.Timeout));

            if (isTimeout)
            {
                _logger.LogError("Lock TIMEOUT ({Timeout}): {LockId}", lockData.Timeout, lockId);

                if (isExceptionOnTimeout)
                {
                    throw new UserException("The functionality is currently busy. Please try again in a moment.", VoteAppErrorCodes.LockTimout);
                }
            }

            return !isTimeout;
        }

        public void ReleaseLock(string lockId)
        {
            var lockData = _semaphors[lockId];
            lockData.Semaphor.Release();
        }
    }
}
