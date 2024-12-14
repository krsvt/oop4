namespace BLL.Service;

using DAL.Entities;
using DAL.Storage.Database;
using BLL.DTO;

public class UnionService
{
    private readonly FamilyTreeDbContext _dbContext;

    public UnionService(FamilyTreeDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public void CreateUnion(UnionDTO union)
    {
        try
        {
            foreach (var c in union.Children)
            {
                var u = new Union
                {
                    Partner1Id = union.Partner[0],
                    Partner2Id = union.Partner[1],
                    ChildId = c
                };
                _dbContext.Unions.Add(u);
            }
            _dbContext.SaveChanges();
        }
        catch (FormatException)
        {
            Console.WriteLine($"Unable to parse");
        }
    }

}
