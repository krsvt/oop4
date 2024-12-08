namespace BLL.DTO;

public class UnionDTO
{
    public string Id { get; set; } = "";
    public List<string> Partner { get; set; } = new List<string>();
    public List<string> Children { get; set; } = new List<string>();
}
