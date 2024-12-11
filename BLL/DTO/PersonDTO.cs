namespace BLL.DTO;

public class PersonDTO
{
    public string Id { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public DateTime BirthYear { get; set; }
    public DateTime? DeathYear { get; set; }
    public List<string> OwnUnions { get; set; } = new List<string>();
    public string? ParentUnion { get; set; }
}
