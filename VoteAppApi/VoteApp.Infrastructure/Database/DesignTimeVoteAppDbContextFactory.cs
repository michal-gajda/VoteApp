using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using System;

namespace VoteApp.Infrastructure.Database
{
    public class DesignTimeVoteAppDbContextFactory : IDesignTimeDbContextFactory<VoteAppDbContext>
    {
        public VoteAppDbContext CreateDbContext(string[] args)
        {
            Console.WriteLine("DesignTimeDbAppContextFactory is running...");
            Console.WriteLine("Conn string is being used from appsettings.design.json");

            IConfigurationRoot config = new ConfigurationBuilder()
                .AddJsonFile("appsettings.design.json")
                .AddEnvironmentVariables()
                .Build();

            var optionsBuilder = new DbContextOptionsBuilder<VoteAppDbContext>();
            optionsBuilder.UseSqlServer(config.GetConnectionString("VOTEAPP"));

            return new VoteAppDbContext(optionsBuilder.Options);
        }
    }
}
