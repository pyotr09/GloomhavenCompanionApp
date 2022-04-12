using Gloom.Model.Interfaces;

namespace Gloom.Model.Monsters
{
    public class Monster : IScenarioParticipant
    {
        public Monster(MonsterGrouping grouping, int level, int number, MonsterTier tier)
        {
            BaseMonsterStats stats = grouping.Type.Stats.GetStatsByLevelAndTier(level, Tier);
            CurrentHitPoints = MaxHitPoints = stats.HitPoints;
            BaseAttack = stats.BaseAttack;
            BaseMove = stats.BaseMove;
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