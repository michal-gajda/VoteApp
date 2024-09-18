using EntityFrameworkCore.Testing.Moq;
using Microsoft.EntityFrameworkCore;
using VoteApp.Infrastructure.Database;

namespace VoteApp.Tests.Common
{
    public static class DatabaseHelpers
    {
        public static VoteAppDbContext CreateInMemoryDbContext(this BaseTestContainer baseCtr, object currentTestClass)
        {
            var options = new DbContextOptionsBuilder<VoteAppDbContext>()
                .UseInMemoryDatabase(databaseName: currentTestClass.GetType().Name)
                .Options;

            var dbContext = Create.MockedDbContextFor<VoteAppDbContext>(options);

            // we can add seed data here

            dbContext.SaveChanges();

            return dbContext;
        }

        public static void ClearDatabase(this VoteAppDbContext dbContext)
        {
            dbContext.RemoveRange(dbContext.Votes);
            dbContext.RemoveRange(dbContext.Persons);

            dbContext.SaveChanges();
        }
    }
}
