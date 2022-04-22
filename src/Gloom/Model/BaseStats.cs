using System.Collections.Generic;
using Gloom.Model.Monsters;

namespace Gloom.Model;

public class BaseStats
{
    /// <summary>
    /// usually just an int, but can be based on other variables for bosses.
    /// examples:
    /// Inox BodyGuard: 1+C, C is number of characters.
    /// Merciless Overseer: V, V is number of scouts present.
    /// Dark Rider: 3+X, X is hexes moved.
    /// </summary>
    public string BaseAttackFormula { get; set; }
    public int Health { get; set; }
    public int BaseTarget { get; set; } = 1;
    public int BaseRange { get; set; }
    public int BaseMove { get; set; }
    public bool IsFlying { get; set; }
    public List<StatusType> StatusesInflicted { get; set; }
    public bool HasAdvantage { get; set; }
    public int BaseShield { get; set; }
    public int BaseRetaliate { get; set; }
    public int BaseRetaliateRange { get; set; }
    public int BasePierce { get; set; }
    public bool DoAttackersGainDisadvantage { get; set; }

    public MonsterTier Tier { get; protected set; }
}