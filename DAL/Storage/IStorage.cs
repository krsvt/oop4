using DAL.Entities;
namespace DAL.Storage;

public interface IStorage
{
    public IRepository<Person> PersonRepository { get; set; }
    public IRepository<Union> UnionRepository { get; set; }
}
