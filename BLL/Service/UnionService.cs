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
                Storage.UnionRepository.AddAsync(u);
            }
        }
        catch (FormatException)
        {
            Console.WriteLine($"Unable to parse");
        }
    }

}
