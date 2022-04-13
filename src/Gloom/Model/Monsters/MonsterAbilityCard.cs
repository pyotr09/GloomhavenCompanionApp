using System;
using System.Collections.Generic;
using System.Linq;

namespace Gloom.Model.Monsters
{
    public class MonsterAbilityCard
    {
        public int Initiative;

        public string ActionsDescription => String.Join("/n", Actions);

        public List<Action> Actions;
        public bool ShuffleAfter;
        public string ImageUrl;
    }
}