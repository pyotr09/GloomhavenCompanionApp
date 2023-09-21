using Gloom.Models.Monsters;

namespace Gloom.Models.Bosses
{
    public class BossType
    {
        public BossType(string name, BossStats stats)
        {
            Name = name;
            Stats = stats;
        }
        public string Name { get; set; }
        public MonsterAbilityDeck AbilityDeck { get; set; }
        public BossStats Stats { get; set; }
    }
}