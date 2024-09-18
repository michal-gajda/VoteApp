using Microsoft.EntityFrameworkCore;
using System.Linq;
using VoteApp.Domain.Voting;

namespace VoteApp.Infrastructure.Database
{
    public class VoteAppDbContext : DbContext
    {
#pragma warning disable CS8618 // Entity Framework takes care of that.
        public VoteAppDbContext(DbContextOptions<VoteAppDbContext> options) : base(options)
#pragma warning restore CS8618 // Entity Framework takes care of that.
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(VoteAppDbContext).Assembly);
        }

        public static void Initialize(VoteAppDbContext dbContext)
        {
            if (!dbContext.Persons.Any())
            {
                dbContext.Persons.AddRange(
                    new Person("John Small").BuildCandidate(),
                    new Person("Michael Bush").BuildCandidate(),
                    new Person("Rita Black").BuildCandidate(),
                    new Person("Tina Trump"),
                    new Person("Kate Smith"),
                    new Person("Thomas Jackson")
                );
            }

            dbContext.SaveChanges();
        }

        public virtual DbSet<Person> Persons { get; set; }
        public DbSet<Vote> Votes { get; set; }
    }
}
