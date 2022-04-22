using Gloom.Model.Monsters;

namespace Gloom.Model.Bosses
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