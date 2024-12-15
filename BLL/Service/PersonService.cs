namespace BLL.Service;

using DAL.Entities;
using DAL.Storage.Database;
using DAL.Storage;
using BLL.DTO;

public class PersonService
{

    private IStorage Storage;
    public PersonService(IStorage storage)
    {
        Storage = storage;
    }

    public async Task CreatePerson(Person person)
    {
        await Storage.PersonRepository.AddAsync(person);
    }

    public Task<List<Person>> GetAll()
    {
        var persons = await Storage.PersonRepository.GetAllAsync();
        return persons.OrderBy(p => p.Id).ToList();
    }

    private PersonDTO MapToDto(Person person)
    {
        return new PersonDTO
        {
            Id = person.Id,
            Name = person.Name,
            BirthYear = person.BirthDate,
            DeathYear = person.DeathDate
        };
    }

    public async Task<RelativesDTO> GetImmediateRelatives(int personId)
    {
        var unions = await Storage.UnionRepository.GetAllAsync();
        var people = await Storage.PersonRepository.GetAllAsync();

        if (!people.Any(p => p.Id == personId))
        {
            throw new ArgumentException($"Person with ID {personId} not found.");
        }

        var relatives = new List<Person>();

        var childrenIds = unions
            .Where(u => (u.Partner1Id == personId || u.Partner2Id == personId))
            .Select(u => u.ChildId);

        relatives.AddRange(people.Where(p => childrenIds.Contains(p.Id)));

        var parentIds = unions
            .Where(u => u.ChildId == personId)
            .SelectMany(u => new[] { u.Partner1Id, u.Partner2Id });

        relatives.AddRange(people.Where(p => parentIds.Contains(p.Id)));

        Console.WriteLine(relatives.Select(MapToDto).ToList().FirstOrDefault());

        var res = new RelativesDTO
        {
            Relatives = relatives.Select(MapToDto).ToList(),
            PersonId = personId
        };
        return res;
    }
}
