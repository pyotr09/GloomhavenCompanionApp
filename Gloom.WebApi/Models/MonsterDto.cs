using Gloom.Common;

namespace Gloom.WebApi.Models;

public class MonsterDto
{
    public int MonsterNumber { get; set; }
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
    public StatusesDto Statuses { get; set; }
}