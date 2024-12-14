namespace BLL.DTO;

public class UnionDTO
{
    public string? Id { get; set; } = "";
    public List<int> Partner { get; set; } = new List<int>();
    public List<int> Children { get; set; } = new List<int>();
}
