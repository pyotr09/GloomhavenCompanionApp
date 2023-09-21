using System.Collections.Generic;
using Gloom.Models.Monsters;

namespace Gloom.Models.Interfaces
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