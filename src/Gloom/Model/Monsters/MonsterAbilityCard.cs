using System;
using System.Collections.Generic;
using System.Linq;

namespace Gloom.Model.Monsters
{
    public class MonsterAbilityCard
    {
        public int Initiative;
        public string Name;
        public string Expansion;
        public string ActionsDescription => String.Join("\n", Actions);
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