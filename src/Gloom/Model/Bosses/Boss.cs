﻿namespace Gloom.Model.Bosses
{
    public class Boss
    {
        public Boss(BossType type, int level, int numberOfCharacters)
        {
            BaseBossStats stats = type.Stats.GetStatsByLevel(level);
            MaxHealth = CalculateHealth(numberOfCharacters, stats.HealthMultiplier);
            // todo: initialize other properties from boss type parameter
        }

        public int BaseAttack { get; set; }
        public int BaseMove { get; set; }
        public int MaxHealth { get; set; }
        // todo: what other properties should Boss have:


        private int CalculateHealth(int numCharacters, int bossHealthMultiplier)
        {
            // todo: Given C and multiplier, calculate and return health

            return -1; // placeholder
        }

        public void RefreshForEndOfRound()
        {
            // todo: what properties change at the end of a round?
        }

        public void RefreshForStartOfTurn()
        {
            // todo: what properties change at the beginning of the boss's turn?
        }

        public void RefreshForEndOfTurn()
        {
            // todo: what properties change at the end of the boss's turn?
        }

        public void SetStatus(StatusType type, bool active, bool currentTurn = false)
        {
            // todo: handle a new status ie. stun/disarm/strengthen/etc.
        }
    }
}