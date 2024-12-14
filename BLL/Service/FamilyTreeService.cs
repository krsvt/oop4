namespace BLL.Service;

using DAL.Entities;
using DAL.Storage.Database;
using Microsoft.EntityFrameworkCore;
using BLL.DTO;

public class FamilyTreeService
{
   private readonly FamilyTreeDbContext _dbContext;

   public FamilyTreeService(FamilyTreeDbContext dbContext)
   {
       _dbContext = dbContext;
   }

public FamilyTreeDTO GetFamilyTree()
{
    var familyTreeDto = new FamilyTreeDTO
    {
        Persons = new Dictionary<int, PersonDTO>(),
        Unions = new Dictionary<string, UnionDTO>(),
        Links = new List<List<string>>()
    };

    var persons = _dbContext.Persons.ToList();

    foreach (var person in persons)
    {
        var personDto = new PersonDTO
        {
            Id = person.Id,
            Name = person.Name,
            BirthYear = person.BirthDate,
            DeathYear = person.DeathDate
        };

        familyTreeDto.Persons[person.Id] = personDto;
    }

    var unions = _dbContext.Unions.ToList();

    foreach (var union in unions)
    {
        var id = union.Partner1Id + "-" + union.Partner1Id;

        if (familyTreeDto.Unions.ContainsKey(id))
        {
            familyTreeDto.Unions[id].Children.Add(union.ChildId);
        }
        else
        {
            var unionDto = new UnionDTO
            {
                Id = id,
                Partner = new List<int>
                {
                    union.Partner1Id,
                    union.Partner2Id
                },
                Children = new List<int> { union.ChildId}
            };
            familyTreeDto.Unions[id] = unionDto;
        }


        if (!familyTreeDto.Persons[union.Partner1Id].OwnUnions.Contains(id)) {
            familyTreeDto.Persons[union.Partner1Id].OwnUnions.Add(id);
            familyTreeDto.Links.Add(new List<string> { union.Partner1Id.ToString(), id });
        }

        if (!familyTreeDto.Persons[union.Partner2Id].OwnUnions.Contains(id))
        {
          familyTreeDto.Persons[union.Partner2Id].OwnUnions.Add(id);
          familyTreeDto.Links.Add(new List<string> { union.Partner2Id.ToString(), id });
        }

        if (union.ChildId != 0)
        {
            familyTreeDto.Links.Add(new List<string> { id, union.ChildId.ToString() });
            familyTreeDto.Persons[union.ChildId].ParentUnion = id;
        }
    }

    // Стартовый элемент (первый в списке)
    familyTreeDto.Start = familyTreeDto.Persons.Keys.FirstOrDefault();

    return familyTreeDto;
}
}
