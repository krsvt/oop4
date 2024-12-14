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

    public void CreatePerson(PersonDTO person)
    {
        var p = new Person
        {
            Gender = Gender.MALE,
            Name = person.Name,
            BirthDate = person.BirthYear,
            DeathDate = person.DeathYear
        };
        Storage.PersonRepository.AddAsync(p);
    }

    public Task<List<Person>> GetAll()
    {
        return Storage.PersonRepository.GetAllAsync();
    }
}
