namespace BLL.Service;

using DAL.Entities;
using DAL.Storage.Database;
using DAL.Storage;
using BLL.DTO;

public class UnionService
{
    private IStorage Storage;

    public UnionService(IStorage storage)
    {
        Storage = storage;
    }

    public async Task CreateUnion(UnionDTO union)
    {
        try
        {
            if (union.Children == null || union.Children.Count == 0)
            {
                var u = new Union
                {
                    Partner1Id = union.Partner[0],
                    Partner2Id = union.Partner[1]
                };
                await Storage.UnionRepository.AddAsync(u);
            }
            else
            {
                foreach (var c in union.Children)
                {
                    var u = new Union
                    {
                        Partner1Id = union.Partner[0],
                        Partner2Id = union.Partner[1],
                        ChildId = c
                    };
                    await Storage.UnionRepository.AddAsync(u);
                }
            }
        }
        catch (FormatException)
        {
            Console.WriteLine($"Unable to parse");
        }
    }

}
