namespace Zoo.Data.Entities;

public class Animal
{
    public int Id { get; set; }
    public string Species { get; set; }
    public string Food { get; set; }
    public int Amount { get; set; }
    public int EnclosureId { get; set; } // Assuming a foreign key to Enclosure

    public Enclosure Enclosure { get; set; }
}