using DAL.Entities;
namespace DAL.Storage.Database;

public class DatabaseStorage : IStorage
{
    public IRepository<Person> PersonRepository { get; set; }
    public IRepository<Union> UnionRepository { get; set; }

    public DatabaseStorage(FamilyTreeDbContext context)
    {
        PersonRepository = new DatabaseRepository<Person>(context);
        UnionRepository = new DatabaseRepository<Union>(context);
    }

    public override string? ToString()
    {
        return "Database storage";
    }
}
