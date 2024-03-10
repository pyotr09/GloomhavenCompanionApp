using Gloom.Common;

namespace Gloom.WebApi.Models;

public class BaseStatsDto
{
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