using Gloom.Model.Monsters;

namespace Gloom.Model.Bosses
{
    public class BossType
    {
        public BossType(BossStats stats)
        {
            Stats = stats;
        }
        public string Name { get; set; }
        public MonsterAbilityDeck AbilityDeck { get; set; }
        public BossStats Stats { get; set; }
    }
}