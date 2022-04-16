using System;
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

        public string BaseAttack { get; set; }
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

        private string CalculateAttack(int numberOfCharacters, string attackFormula, int numberOfScouts)
        {
            if (attackFormula == 'V'.ToString() || attackFormula.Contains('X')) 
            {
                return attackFormula;
            }
            if (int.TryParse(attackFormula, out var basic))
            {
                return basic.ToString();
            }
            if (int.TryParse(attackFormula.Substring(0,attackFormula.IndexOf('+')), out var adder))
            {
                return (adder + numberOfCharacters).ToString();
            }
            if (!int.TryParse(attackFormula.Substring(0, attackFormula.IndexOf('x')), out var multiplier))
            {
                throw new Exception($"Failed to determine boss attack");
            }
            return (multiplier * numberOfCharacters).ToString();
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