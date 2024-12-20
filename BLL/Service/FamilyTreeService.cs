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
            var id = union.Partner1Id + "-" + union.Partner2Id;

            if (familyTreeDto.Unions.ContainsKey(id) && union.ChildId != 0)
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
                }
                };
                unionDto.Children = new List<int>();
                if (union.ChildId != 0) unionDto.Children.Add(union.ChildId);
                familyTreeDto.Unions[id] = unionDto;
            }


            if (!familyTreeDto.Persons[union.Partner1Id].OwnUnions.Contains(id))
            {
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

        familyTreeDto.Start = familyTreeDto.Persons.Keys.FirstOrDefault();

        return familyTreeDto;
    }

    public static int? CalculateAgeAtChildBirth(Person parent, Person child)
    {
        if (parent.BirthDate > child.BirthDate)
        {
            return null;
        }

        return (child.BirthDate.Year - parent.BirthDate.Year) -
               (child.BirthDate < parent.BirthDate.AddYears(child.BirthDate.Year - parent.BirthDate.Year) ? 1 : 0);
    }


    public int? GetAncestorAgeAtChildBirth(ChildAndAncestorDTO dto)
    {
        Person c = _dbContext.Persons.Where(p => dto.ChildId == p.Id).FirstOrDefault();
        Person p = _dbContext.Persons.Where(p => dto.AncestorId == p.Id).FirstOrDefault();
        return CalculateAgeAtChildBirth(p, c);
    }


    public void DeleteTree()
    {
        _dbContext.Unions.RemoveRange(_dbContext.Unions.ToList());
        _dbContext.Persons.RemoveRange(_dbContext.Persons.ToList());
        _dbContext.SaveChanges();
    }
}
