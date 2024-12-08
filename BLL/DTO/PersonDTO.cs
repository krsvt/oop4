namespace BLL.DTO;

public class PersonDTO
{
    public string Id { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public int BirthYear { get; set; }
    public int? DeathYear { get; set; }
    public List<string> OwnUnions { get; set; } = new List<string>();
    public string? ParentUnion { get; set; }
}
