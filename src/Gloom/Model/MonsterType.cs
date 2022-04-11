using System.Collections.Generic;
using Gloom.Model.Interfaces;

namespace Gloom.Model
{
    public class MonsterType
    {
        public string Name { get; set; }
        public string ImageUrl { get; set; }
        public MonsterAbilityDeck AbilityDeck { get; set; }
        public MonsterStats Stats { get; set; }
        public bool IsFlying { get; set; }
        public int MaxNumberOnBoard { get; set; }
    }

    public class MonsterStats
    {
        public BaseStats GetStatsByLevelAndTier(int level, MonsterTier tier)
        {
            if (tier == MonsterTier.Normal)
            {
                return StatsByLevel_Normal[level];
            }
            if (tier == MonsterTier.Elite)
            {
                return StatsByLevel_Elite[level];
            }

            return null;
        }
        public BaseStats[] StatsByLevel_Normal { get; set; }
        public BaseStats[] StatsByLevel_Elite { get; set; }
    }

    public class BaseStats
    {
        public int HitPoints { get; set; }
        public int BaseAttack { get; set; }
        public int BaseMove { get; set; }
        public int BaseShield { get; set; }
        public int BaseRetaliate { get; set; }
        
    }
    
    public class MonsterAbilityDeck : AbstractCardDeck<MonsterAbilityCard>
    {
        public MonsterAbilityDeck(List<MonsterAbilityCard> cards) : base(cards)
        {
        }
    }
    
    public class MonsterAbilityCard
    {
        public int Initiative;
        public string ActionsDescription;
        public bool ShuffleAfter;
        public string ImageUrl;
    }
}