using DAL.Entities;
namespace DAL.Storage.Database;

public class DatabaseStorage : IStorage
{

    // public IRepository<Product> ProductRepository { get; set; }
    // public IRepository<Shop> ShopRepository { get; set; }
    // public IRepository<ShopProducts> ShopProductsRepository { get; set; }
    //
    public DatabaseStorage(FamilyTreeDbContext context)
    {
        // ProductRepository = new DatabaseRepository<Product>(context);
        // ShopRepository = new DatabaseRepository<Shop>(context);
        // ShopProductsRepository = new DatabaseRepository<ShopProducts>(context);
    }

    public override string? ToString()
    {
        return "Database storage";
    }
}
