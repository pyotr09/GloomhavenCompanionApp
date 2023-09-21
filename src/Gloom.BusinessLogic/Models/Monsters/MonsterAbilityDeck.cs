using System.Collections.Generic;

namespace Gloom.Models.Monsters
{
    public class MonsterAbilityDeck : AbstractCardDeck<MonsterAbilityCard>
    {
        public MonsterAbilityDeck() : base()
        {
        }
        public MonsterAbilityDeck(List<MonsterAbilityCard> cards) : base(cards)
        {
        }
    }
}