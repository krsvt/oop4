using System.Collections.Generic;

namespace DAL.Entities;

public class Person : BaseEntity
{

  public enum GenderType {
    MALE,
    FEMALE
  }

  public string Name { get; set; } = "";
  public GenderType Gender { get; set; }
  public DateTime BirthDate { get; set; }
  public DateTime? DeathDate { get; set; }
}
