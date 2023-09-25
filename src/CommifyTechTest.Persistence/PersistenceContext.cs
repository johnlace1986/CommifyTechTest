using CommifyTechTest.Persistence.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace CommifyTechTest.Persistence;

internal class PersistenceContext : DbContext
{
    public PersistenceContext() : base()
    {
    }

    public DbSet<Employee> Employees { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        if (optionsBuilder.IsConfigured is false)
        {
            IConfigurationRoot configuration = new ConfigurationBuilder()
                .SetBasePath(AppDomain.CurrentDomain.BaseDirectory)
                .AddJsonFile("persistencesettings.json")
                .Build();

            optionsBuilder.UseSqlServer(
                connectionString: configuration.GetSection("sql:connectionString").Value);
        }

        base.OnConfiguring(optionsBuilder);
    }
}
