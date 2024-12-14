using System.Collections.Generic;

namespace DAL.Entities;

public class Person : BaseEntity
{
  public int Id {get; set;}
  public string Name { get; set; } = "";
  public Gender Gender { get; set; }
  public DateTime BirthDate { get; set; }
  public DateTime? DeathDate { get; set; }
}
