using Gloom.Common;
using Gloom.Models.Interfaces;

namespace Gloom.Models.Monsters
{
    public class Monster : IScenarioParticipant
    {
        public Monster()
        {
            
        }

        public Monster(string name, int level, int number, MonsterTier tier)
        {
            MonsterType type = new MonsterType(name, Utils.GetDeckName(name, "Gloomhaven"));
            var stats = type.Stats.GetStatsByLevelAndTier(level, tier);
            
            CurrentHitPoints = MaxHitPoints = stats.Health;
            BaseAttack = int.Parse(stats.BaseAttackFormula);
            BaseMove = stats.BaseMove;
            BaseRange = stats.BaseRange;
            IsFlying = stats.IsFlying;
            CurrentShield = BaseShield = stats.BaseShield;
            CurrentRetaliate = BaseRetaliate = stats.BaseRetaliate;
            BaseRetaliateRange = stats.BaseRetaliateRange;
            DoAttackersGainDisadvantage = stats.DoAttackersGainDisadvantage;
            Statuses = new Statuses();
            MonsterNumber = number;
            Tier = tier;
        }


        public Monster(BaseMonsterStats stats, int number, MonsterTier tier)
        {
            CurrentHitPoints = MaxHitPoints = stats.Health;
            BaseAttack = int.Parse(stats.BaseAttackFormula);
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
        public int BaseRetaliateRange { get; set; } 
        public int CurrentRetaliate { get; set; }
        public int MaxHitPoints { get; set; }
        public int CurrentHitPoints { get; set; }
        public MonsterTier Tier { get; set; }
        public bool DoAttackersGainDisadvantage { get; set; }

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
}