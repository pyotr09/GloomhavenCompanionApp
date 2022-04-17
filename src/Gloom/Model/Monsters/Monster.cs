﻿using Gloom.Model.Interfaces;

namespace Gloom.Model.Monsters
{
    public class Monster : IScenarioParticipant
    {
        public Monster(BaseMonsterStats stats, int number, MonsterTier tier)
        {
            CurrentHitPoints = MaxHitPoints = stats.Health;
            BaseAttack = stats.BaseAttack;
            BaseMove = stats.BaseMove;
            BaseRange = stats.BaseRange;
            IsFlying = stats.IsFlying;
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
        public int BaseRange { get; set; }
        public bool IsFlying { get; set; }
        public int BaseShield { get; set; }
        public int CurrentShield { get; set; }
        public int BaseRetaliate { get; set; } 
        // still need to handle ranged retaliate, and having both melee and ranged retaliate
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

        public void RefreshForStartOfTurn()
        {
            // if wounded/regenerate, etc.
        }
    }

    public enum MonsterTier
    {
        Boss, Named, Elite, Normal
    }
}