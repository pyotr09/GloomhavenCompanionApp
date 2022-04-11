using System;
using System.Collections.Generic;
using Gloom.CustomExceptions;
using Gloom.Model.Interfaces;

namespace Gloom.Model.Monsters
{
public class MonsterGrouping : IScenarioParticipantGroup
    {
        public MonsterType Type;

        public MonsterGrouping(MonsterType type)
        {
            Type = type;
            Initiative = null;
            Monsters = new List<Monster>();
            _activeMonsterNumbers = new List<int>(type.MaxNumberOnBoard);
            _availableMonsterNumbers = new List<int>(type.MaxNumberOnBoard);
            for (int i = 1; i <= type.MaxNumberOnBoard; i++)
            {
                _availableMonsterNumbers.Add(i);
            }
        }

        private static Random r = new Random();

        public int? Initiative { get; set; }
        public string Name => Type.Name;
        public MonsterAbilityDeck AbilityDeck => Type.AbilityDeck;
        public string ActionText { get; set; }
        public List<Monster> Monsters { get; set; }

        public void AddMonster(MonsterTier tier, int level)
        {
            int num = GetNewMonsterNumber();
            Monsters.Add(new Monster(this, level, num, tier));
            _activeMonsterNumbers.Add(num);
            _availableMonsterNumbers.Remove(num);
        }

        public void RemoveMonster(int number)
        {
            Monsters.RemoveAll(m => m.MonsterNumber == number);
            _activeMonsterNumbers.Remove(number);
            _availableMonsterNumbers.Add(number);
        }

        private readonly List<int> _activeMonsterNumbers;
        private readonly List<int> _availableMonsterNumbers;
        private int GetNewMonsterNumber()
        {
            if (_activeMonsterNumbers.Count == Type.MaxNumberOnBoard)
                throw new AllMonsterNumbersUsedException("ALl Monster Numbers Used", Type.Name); 
            int randomIndex = r.Next(1, _availableMonsterNumbers.Count);
            return _availableMonsterNumbers[randomIndex - 1];
        }

        public void DrawNewAbility()
        {
            var newAbility = AbilityDeck.Draw();
            Initiative = newAbility.Initiative;
            ActionText = newAbility.ActionsDescription;
        }

        public void RefreshForEndOfRound()
        {
            
        }
    }
}