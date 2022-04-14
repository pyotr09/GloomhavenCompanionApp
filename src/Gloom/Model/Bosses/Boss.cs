using System.Collections.Generic;

namespace Gloom.Model.Bosses
{
    public class Boss
    {
        public Boss(BossType type, int level, int numberOfCharacters, int numberOfScouts = 0)
        {
            BaseBossStats stats = type.Stats.GetStatsByLevel(level);
            MaxHealth = CalculateHealth(numberOfCharacters, stats.HealthMultiplier);
            BaseAttack = CalculateAttack(numberOfCharacters, stats.AttackFormula, numberOfScouts);
            BaseRange = stats.BaseRange;
            BaseMove = stats.BaseMove;
            Immunities = stats.Immunities;
            Special1Actions = stats.Special1Actions;
            Special2Actions = stats.Special2Actions;
            Notes = stats.Notes;
            // todo: initialize other properties from boss type parameter
        }

        public int BaseAttack { get; set; }
        public int BaseMove { get; set; }
        public int MaxHealth { get; set; }
        public int BaseRange { get; set; }
        public List<StatusType> Immunities { get; set; }
        public List<string> Special1Actions { get; set; }
        public List<string> Special2Actions { get; set; }
        public List<string> Notes { get; set; }
        
        // todo: what other properties should Boss have:


        private int CalculateHealth(int numCharacters, int bossHealthMultiplier)
        {
            return numCharacters * bossHealthMultiplier;
        }

        private int CalculateAttack(int numberOfCharacters, string attackFormula, int numberOfScouts)
        {
            // todo: determine parameters which will affect damage, parse 'AttackFormula' string
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