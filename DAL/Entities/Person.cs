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
  public int BirthYear { get; set; }
  public int? DeathYear { get; set; }
}
