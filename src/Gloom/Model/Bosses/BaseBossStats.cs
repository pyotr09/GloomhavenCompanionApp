using System.Collections.Generic;

namespace Gloom.Model.Bosses
{
    public class BaseBossStats
    {
        /// <summary>
        /// all bosses in base Gloomhaven have health as int * C,
        /// where C is number of characters
        /// </summary>
        public int HealthMultiplier { get; set; }
        
        /// <summary>
        /// usually just an int, but can be based on other variables.
        /// examples:
        /// Inox BodyGuard: 1+C, C is number of characters.
        /// Merciless Overseer: V, V is number of scouts present.
        /// Dark Rider: 3+X, X is hexes moved.
        /// </summary>
        public string AttackFormula { get; set; }
        
        public int BaseRange { get; set; }
        public int BaseMove { get; set; }
        
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