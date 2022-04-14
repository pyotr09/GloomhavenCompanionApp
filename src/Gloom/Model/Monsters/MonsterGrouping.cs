using System;
using System.Collections.Generic;
using Gloom.CustomExceptions;
using Gloom.Model.Actions;
using Gloom.Model.Interfaces;

namespace Gloom.Model.Monsters
{
public class MonsterGrouping : IScenarioParticipantGroup
    {
        public MonsterGrouping(MonsterType type, int level)
        {
            Name = type.Name;
            DeckName = type.DeckName;
            NormalStats = type.Stats.GetStatsByLevelAndTier(level, MonsterTier.Normal);
            EliteStats = type.Stats.GetStatsByLevelAndTier(level, MonsterTier.Elite);
            Initiative = null;
            Monsters = new List<Monster>();
            _activeMonsterNumbers = new List<int>(type.MaxNumberOnBoard);
            _availableMonsterNumbers = new List<int>(type.MaxNumberOnBoard);
            for (int i = 1; i <= type.MaxNumberOnBoard; i++)
            {
                _availableMonsterNumbers.Add(i);
            }

            MaxNumberOnBoard = type.MaxNumberOnBoard;

            AbilityDeck = AbilityParser.Instance.ParseDeck(this);
        }

        private static Random r = new Random();
        
        public int MaxNumberOnBoard { get; set; }
        public BaseMonsterStats NormalStats;
        public BaseMonsterStats EliteStats;
        public int? Initiative { get; set; }
        public string Name { get; set; }
        public string DeckName { get; set; }
        public MonsterAbilityDeck AbilityDeck { get; set; }
        public string ActionText { get; set; }
        public List<Monster> Monsters { get; set; }

        public void AddMonster(MonsterTier tier, int? num = null)
        {
            num ??= GetNewMonsterNumber();
            var stats = tier == MonsterTier.Elite ? EliteStats : NormalStats;
            Monsters.Add(new Monster(stats, num.Value, tier));
            _activeMonsterNumbers.Add(num.Value);
            _availableMonsterNumbers.Remove(num.Value);
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
            if (_activeMonsterNumbers.Count == MaxNumberOnBoard)
                throw new AllMonsterNumbersUsedException("ALl Monster Numbers Used", Name); 
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