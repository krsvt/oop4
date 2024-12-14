namespace BLL.DTO;

public class FamilyTreeDTO
{
    public int Start { get; set; }
    public Dictionary<int, PersonDTO> Persons { get; set; } = new Dictionary<int, PersonDTO>();
    public Dictionary<string, UnionDTO> Unions { get; set; } = new Dictionary<string, UnionDTO>();
    public List<List<string>> Links { get; set; } = new List<List<string>>();
}
