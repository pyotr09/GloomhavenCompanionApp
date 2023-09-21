using System.Collections.Generic;

namespace Gloom.Models.Monsters
{
    public class MonsterAbilityCard
    {
        public int Initiative;
        public string Name;
        public string Expansion;
        public List<ActionSet> Actions;
        public bool ShuffleAfter;
        public string ImagePath;
    }

    public class ActionSet
    {
        public string BaseActionText;
        public string NormalActionText;
        public string EliteActionText;
    }
}