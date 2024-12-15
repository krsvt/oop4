using DAL.Entities;

namespace BLL.DTO;

public record PersonDTO
{
    public int? Id { get; set; }
    public string Name { get; set; }
    public Gender Gender { get; set; }
    public DateTime BirthYear { get; set; }
    public DateTime? DeathYear { get; set; }
    public List<string> OwnUnions { get; set; } = new List<string>();
    public string? ParentUnion { get; set; }
}
