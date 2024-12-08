using Microsoft.EntityFrameworkCore;
using DAL.Storage;
using DAL.Storage.Database;

namespace Representation.Configuration;
public static class StorageExtensions
{
  public static void AddStorage(this IServiceCollection services, IConfiguration configuration)
  {
    var dbaType = configuration["DBA"];

    if (dbaType == "db")
    {
      ConfigureDatabaseStorage(services, configuration);
    }
    else
    {
      throw new Exception("NO DBA IN CONFIGURATION");
    }
  }

  private static void ConfigureDatabaseStorage(IServiceCollection services, IConfiguration configuration)
  {
    var dbConfig = configuration.GetConnectionString("DefaultConnection");
    services.AddDbContext<FamilyTreeDbContext>(options =>
        options.UseNpgsql(dbConfig).EnableSensitiveDataLogging());
    services.AddScoped<IStorage, DatabaseStorage>();
    Console.WriteLine("Running with Database storage");
  }
}
