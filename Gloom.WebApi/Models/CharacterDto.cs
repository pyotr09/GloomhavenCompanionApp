namespace Gloom.WebApi.Models;

public class CharacterDto
{
    public int CurrentHealth { get; set; }
    public int MaxHealth { get; set; }
    public int Level { get; set; }
    public string Name { get; set; }
    public int Xp { get; set; }
    public int Gold { get; set; }
    public int? Initiative { get; set; }
    public string Type => "Character";
    public string DeckName => "";
}