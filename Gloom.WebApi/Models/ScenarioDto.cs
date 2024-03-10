using Gloom.Common;

namespace Gloom.WebApi.Models;

public class ScenarioDto
{
    public int Level { get; set; }
    public string Name { get; set; }
    public int NumCharacters { get; set; }
    public List<MonsterGroupDto> MonsterGroups { get; set; }
    public List<CharacterDto> Characters { get; set; }
    public List<BossDto> Bosses { get; set; }
    public bool IsBetweenRounds { get; set; }
    public Dictionary<Element, int> Elements { get; set; }
}