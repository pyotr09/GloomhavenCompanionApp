using System.Collections.Generic;

namespace Gloom.Model
{
    public class MonsterType
    {
        public string Name { get; set; }
        public string ImageUrl { get; set; }
        public MonsterAbilityDeck AbilityDeck { get; set; }
        public MonsterStats Stats { get; set; }
        public bool IsFlying { get; set; }
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
    
    public class MonsterAbilityDeck
    {
        public MonsterAbilityDeck(List<MonsterAbilityCard> cards)
        {
            Cards = cards;
            _drawPile = new Stack<MonsterAbilityCard>(cards);
            _discardPile = new Stack<MonsterAbilityCard>();
        }
        
        public string Name;
        public List<MonsterAbilityCard> Cards;

        private Stack<MonsterAbilityCard> _drawPile;
        private Stack<MonsterAbilityCard> _discardPile;

        public void Draw()
        {
            
        }
        
        public void Shuffle()
        {
            
        }
    }
    
    public class MonsterAbilityCard
    {
        public int Initiative;
        public string ActionsDescription;
        public string ImageUrl;
    }
}