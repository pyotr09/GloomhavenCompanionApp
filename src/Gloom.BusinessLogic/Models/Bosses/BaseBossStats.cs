using System.Collections.Generic;
using Gloom.Models.Monsters;

namespace Gloom.Models.Bosses
{
    public class BaseBossStats : BaseStats
    {
        public BaseBossStats()
        {
            Tier = MonsterTier.Boss;
        }
        /// <summary>
        /// all bosses in base Gloomhaven have health as int * C,
        /// where C is number of characters
        /// </summary>
        public int HealthMultiplier { get; set; }

        /// <summary>
        /// Statuses to which boss is immune
        /// </summary>
        public List<StatusType> Immunities { get; set; }
        
        /// <summary>
        /// Extra notes on the Boss stat card
        /// </summary>
        public List<string> Notes { get; set; }
        
        public List<string> Special1Actions { get; set; }
        public List<string> Special2Actions { get; set; }
    }
}