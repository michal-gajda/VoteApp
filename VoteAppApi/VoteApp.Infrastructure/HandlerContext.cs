using System.Threading.Tasks;
using VoteApp.Application;

namespace VoteApp.Infrastructure
{
    public class HandlerContext : IHandlerContext
    {
        private bool? _isForceCompleteTransaction;
        public bool? IsForceCompleteTransaction => _isForceCompleteTransaction;
        public void ForceCompleteTransaction() => _isForceCompleteTransaction = true;

        private string? _lockKey;
        public string? LockId => _lockKey;
        public void ConfigureLock(string lockKey)
        {
            _lockKey = lockKey;
        }

        private bool _dontUseCommandValidator;
        public bool IsDontUseCommandValidator => _dontUseCommandValidator;

        public void DontUseCommandValidator()
        {
            _dontUseCommandValidator = true;
        }
    }
}
