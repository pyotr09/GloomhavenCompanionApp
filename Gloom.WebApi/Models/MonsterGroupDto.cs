namespace Gloom.WebApi.Models;

public class MonsterGroupDto
{
    public List<BaseStatsDto> BaseStatsList { get; set; }
    public MonsterAbilityDeckDto AbilityDeck { get; set; }
    public int Count { get; set; }
    public int? Initiative { get; set; }
    public string Name { get; set; }
    public List<MonsterDto> Monsters { get; set; }
    public MonsterAbilityCardDto ActiveAbilityCard { get; set; }
}