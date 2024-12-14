using DAL.Entities;
using Microsoft.EntityFrameworkCore;

namespace DAL.Storage.Database;

public class FamilyTreeDbContext : DbContext
{
    public FamilyTreeDbContext(DbContextOptions<FamilyTreeDbContext> options)
        : base(options)
    {
    }

    public DbSet<Person> Persons { get; set; } = null;
    public DbSet<Union> Unions { get; set; } = null;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfiguration(new UnionsConfiguration());
    }
}
