using System;
using System.Threading.Tasks;
using System.Transactions;
using VoteApp.Application;
using VoteApp.Domain;

namespace VoteApp.Infrastructure.Database.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly IAppConfig _appConfig;
        private readonly VoteAppDbContext _dbContext;

        public UnitOfWork(IAppConfig appConfig, VoteAppDbContext dbContext)
        {
            _appConfig = appConfig;
            _dbContext = dbContext;
        }

        public IBaseRepository<T> CreateRepository<T>()
            where T : class
        {
            return new Repository<T>(_dbContext);
        }

        public async Task CommitAsync()
        {
            await _dbContext.SaveChangesAsync();
        }

        public async Task CommitCurrentTransactionAsync()
        {
            if (_dbContext.Database.CurrentTransaction != null)
            {
                await _dbContext.Database.CurrentTransaction.CommitAsync();
            }
        }

        /// <summary>
        /// Creates async safe transaction scope with specified timeout
        /// </summary>
        /// <returns></returns>
        public TransactionScope CreateTransactionScope(int? minutesTimeout = null, TransactionScopeOption option = TransactionScopeOption.Required)
        {
            return new TransactionScope(option,
                new TransactionOptions { IsolationLevel = IsolationLevel.ReadCommitted, Timeout = minutesTimeout.HasValue && minutesTimeout.Value > 0 ? TimeSpan.FromMinutes(minutesTimeout.Value) : _appConfig.MainSettings.DbTransactionTimeout },
                TransactionScopeAsyncFlowOption.Enabled
                );
        }
    }
}
