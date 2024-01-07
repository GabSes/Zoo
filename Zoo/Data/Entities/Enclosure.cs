namespace Zoo.Data.Entities;

public class Enclosure
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Size { get; set; }
    public string Location { get; set; }
    public string[] Objects { get; set; }
    public string[] AllowedSpecies { get; set; }

    public List<Animal> Animals { get; set; } = new List<Animal>();
}