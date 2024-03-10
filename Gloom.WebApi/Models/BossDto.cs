using Gloom.Common;

namespace Gloom.WebApi.Models;

public class BossDto
{
    public bool IsActive { get; set; }
    public string BaseAttack { get; set; }
    public int BaseMove { get; set; }
    public int CurrentHealth { get; set; }
    public int MaxHealth { get; set; }
    public int BaseRange { get; set; }
    public List<StatusType> Immunities { get; set; }
    public List<string> Special1Actions { get; set; }
    public List<string> Special2Actions { get; set; }
    public List<string> Notes { get; set; }
    public int? Initiative { get; set; }
    public string Name { get; set; }
    public MonsterAbilityDeckDto AbilityDeck { get; set; }
    public MonsterAbilityCardDto ActiveAbilityCard { get; set; }
    public string DeckName { get; set; }
    public List<BaseStatsDto> BaseStatsList { get; set; }
}