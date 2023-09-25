using Microsoft.EntityFrameworkCore;
using CommifyTechTest.Persistence.Models;

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
            optionsBuilder.UseSqlServer(
                connectionString: "Server=localhost;Database=CommifyTechTest;Trusted_Connection=True;Encrypt=False");
        }

        base.OnConfiguring(optionsBuilder);
    }
}
