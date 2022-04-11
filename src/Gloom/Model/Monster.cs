using System;
using System.Collections.Generic;
using Gloom.Model.Interfaces;

namespace Gloom.Model
{
    public class MonsterGrouping : IScenarioParticipantGroup
    {
        public MonsterType Type;

        public MonsterGrouping(MonsterType type)
        {
            Type = type;
            Initiative = null;
            _activeMonsterNumbers = new List<int>(type.MaxNumberOnBoard);
            _availableMonsterNumbers = new List<int>(type.MaxNumberOnBoard);
            for (int i = 1; i <= type.MaxNumberOnBoard; i++)
            {
                _availableMonsterNumbers.Add(i);
            }
        }

        private static Random r;

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
                return -1; // todo throw custom exception
            int randomIndex = r.Next(1, _availableMonsterNumbers.Count);
            return _availableMonsterNumbers[randomIndex];
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
    
    public class Monster : IScenarioParticipant
    {
        public Monster(MonsterGrouping grouping, int level, int number, MonsterTier tier)
        {
            var stats = grouping.Type.Stats.GetStatsByLevelAndTier(level, Tier);
            CurrentHitPoints = MaxHitPoints = stats.HitPoints;
            BaseAttack = stats.BaseAttack;
            BaseMove = stats.BaseMove;
            IsFlying = grouping.Type.IsFlying;
            CurrentShield = BaseShield = stats.BaseShield;
            CurrentRetaliate = BaseRetaliate = stats.BaseRetaliate;
            Statuses = new Statuses();
            MonsterNumber = number;
            Tier = tier;
        }
        
        public int MonsterNumber { get; set; }
        public Statuses Statuses { get; set; }
        public int BaseAttack { get; set; }
        public int BaseMove { get; set; }
        public bool IsFlying { get; set; }
        public int BaseShield { get; set; }
        public int CurrentShield { get; set; }
        public int BaseRetaliate { get; set; }
        public int CurrentRetaliate { get; set; }
        public int MaxHitPoints { get; set; }
        public int CurrentHitPoints { get; set; }
        public MonsterTier Tier { get; set; }

        public void RefreshForEndOfRound()
        {
            CurrentShield = BaseShield;
            CurrentRetaliate = BaseRetaliate;
        }

        public void SetStatus(StatusType type, bool active, bool currentTurn = false)
        {
            Statuses.SetStatus(type, active, currentTurn);
        }

        public void RefreshForEndOfTurn()
        {
            Statuses.ClearForEndOfTurn();
        }

        public void StartTurn()
        {
            // if wounded/regenerate, etc.
        }
    }

    public enum MonsterTier
    {
        Normal, Elite, Boss, Named
    }
}