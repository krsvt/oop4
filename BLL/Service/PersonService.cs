namespace BLL.Service;

using DAL.Entities;
using DAL.Storage.Database;
using BLL.DTO;

public class PersonService
{
   private readonly FamilyTreeDbContext _dbContext;

   public PersonService(FamilyTreeDbContext dbContext)
   {
       _dbContext = dbContext;
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
     _dbContext.Persons.Add(p);
     _dbContext.SaveChanges();
   }
}
