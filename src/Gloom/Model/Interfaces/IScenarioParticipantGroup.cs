using System.Collections.Generic;
using Gloom.Model.Monsters;

namespace Gloom.Model.Interfaces
{
    public interface IScenarioParticipantGroup
    {
        int? Initiative { get; }
        string Name { get; }
        void Draw();
        string Type { get; }
        string DeckName { get; }
        List<BaseStats> BaseStatsList { get; }
        public MonsterAbilityDeck AbilityDeck { get;  }
        void RefreshForEndOfRound();
    }
}