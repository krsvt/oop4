namespace BLL.DTO;

public class FamilyTreeDTO
{
    public string Start { get; set; } = string.Empty;
    public Dictionary<string, PersonDTO> Persons { get; set; } = new Dictionary<string, PersonDTO>();
    public Dictionary<string, UnionDTO> Unions { get; set; } = new Dictionary<string, UnionDTO>();
    public List<List<string>> Links { get; set; } = new List<List<string>>();
}
