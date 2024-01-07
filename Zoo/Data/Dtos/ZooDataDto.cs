namespace Zoo.Data.Dtos;

// DTOs for receiving JSON data
public class ZooDataDto
{
    public List<EnclosureDto> Enclosures { get; set; }
    public List<AnimalDto> Animals { get; set; }
}

public class EnclosureDto
{
    public string Name { get; set; }
    public string Size { get; set; }
    public string Location { get; set; }
    public string[] Objects { get; set; }
    public string[] AllowedSpecies { get; set; }
}

public class AnimalDto
{
    public string Species { get; set; }
    public string Food { get; set; }
    public int Amount { get; set; }
    public int EnclosureId { get; set; }
}
