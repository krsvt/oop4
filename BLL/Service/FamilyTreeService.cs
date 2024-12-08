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
        Persons = new Dictionary<string, PersonDTO>(),
        Unions = new Dictionary<string, UnionDTO>(),
        Links = new List<List<string>>()
    };

    var persons = _dbContext.Persons.ToList();

    foreach (var person in persons)
    {
        var personDto = new PersonDTO
        {
            Id = person.Id.ToString(),
            Name = person.Name,
            BirthYear = person.BirthYear,
            DeathYear = person.DeathYear
        };

        familyTreeDto.Persons[person.Id.ToString()] = personDto;
    }

    var unions = _dbContext.Unions.ToList();

    // Обрабатываем союзы
    foreach (var union in unions)
    {
        var id = "u" + union.Id.ToString();

        if (familyTreeDto.Unions.ContainsKey(id))
        {
            familyTreeDto.Unions[id].Children.Add(union.ChildId.ToString());
        }
        else
        {
            var unionDto = new UnionDTO
            {
                Id = id,
                Partner = new List<string>
                {
                    union.Partner1Id.ToString(),
                    union.Partner2Id.ToString()
                },
                Children = new List<string> { union.ChildId.ToString() }
            };
            familyTreeDto.Unions[id] = unionDto;
        }


        if (!familyTreeDto.Persons[union.Partner1Id.ToString()].OwnUnions.Contains(id)) {
            familyTreeDto.Persons[union.Partner1Id.ToString()].OwnUnions.Add(id);
            familyTreeDto.Links.Add(new List<string> { union.Partner1Id.ToString(), id });
        }

        if (!familyTreeDto.Persons[union.Partner2Id.ToString()].OwnUnions.Contains(id))
        {
          familyTreeDto.Persons[union.Partner2Id.ToString()].OwnUnions.Add(id);
          familyTreeDto.Links.Add(new List<string> { union.Partner2Id.ToString(), id });
        }

        if (union.ChildId != 0)
        {
            familyTreeDto.Links.Add(new List<string> { id, union.ChildId.ToString() });
            familyTreeDto.Persons[union.ChildId.ToString()].ParentUnion = id;
        }
    }

    // Стартовый элемент (первый в списке)
    familyTreeDto.Start = familyTreeDto.Persons.Keys.FirstOrDefault();

    return familyTreeDto;
}
}
