using VoteApp.Application;
using Microsoft.EntityFrameworkCore;

namespace VoteApp.Infrastructure.Database.Repositories
{
    public class UnitOfWorkFactory : IUnitOfWorkFactory
    {
        private readonly IAppConfig _appConfig;
        private readonly DbContextOptions<VoteAppDbContext> _dbVoteAppOptions;

        public UnitOfWorkFactory(IAppConfig appConfig, DbContextOptions<VoteAppDbContext> dbVoteAppOptions)
        {
            _appConfig = appConfig;
            _dbVoteAppOptions = dbVoteAppOptions;
        }

        public IUnitOfWork CreateUnitOfWork()
        {
            return new UnitOfWork(_appConfig, new VoteAppDbContext(_dbVoteAppOptions));
        }
    }
}
