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
        return Storage.PersonRepository.GetAllAsync();
    }
}
