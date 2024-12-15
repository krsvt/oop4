namespace BLL.DTO;

public record RelativesDTO
{
    public int PersonId { get; set; }
    public List<PersonDTO> Relatives {get; set;}  = new List<PersonDTO>();
}
