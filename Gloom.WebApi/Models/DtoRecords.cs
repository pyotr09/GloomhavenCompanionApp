using Gloom.Common;

namespace Gloom.WebApi.Models;

public record SetScenarioDto(int Number, int Level);
public record AddMonsterDto(string GroupName, string Tier, int Number);
public record AddCharacterDto(string Name, int Level);
public record SetInitiativeDto(string Name, int Initative);
public record UpdateMonsterStateDto(string GroupName, int MonsterNumber, int NewHp, Dictionary<StatusType, bool> Statuses);
public record RemoveMonsterDto(string GroupName, int Number);
public record DrawAbilityForGroupDto(string GroupName);
public class StatusDto
{
    public StatusDto()
    {
        
    }
    public StatusDto(bool IsActive, StatusType Type)
    {
        this.IsActive = IsActive;
        this.Type = Type;
    }

    public bool IsActive { get; init; }
    public StatusType Type { get; init; }

    public void Deconstruct(out bool IsActive, out StatusType Type)
    {
        IsActive = this.IsActive;
        Type = this.Type;
    }
}

public class StatusesDto
{
    public StatusesDto()
    {
        this.Stun = new StatusDto(false, StatusType.Stun);
        this.Disarm = new StatusDto(false, StatusType.Disarm);
        this.Immobilize = new StatusDto(false, StatusType.Immobilize);
        this.Poison = new StatusDto(false, StatusType.Poison);
        this.Wound = new StatusDto(false, StatusType.Wound);
        this.Strengthen = new StatusDto(false, StatusType.Strengthen);
        this.Muddle = new StatusDto(false, StatusType.Muddle);
        this.Regenerate = new StatusDto(false, StatusType.Regenerate);
        this.Invisible = new StatusDto(false, StatusType.Invisible);
        
    }
    public StatusesDto(StatusDto Stun, StatusDto Disarm, StatusDto Immobilize, 
        StatusDto Poison, StatusDto Wound, StatusDto Strengthen, StatusDto Muddle, StatusDto Regnerate,
        StatusDto Invisible)
    {
        this.Stun = Stun;
        this.Disarm = Disarm;
        this.Immobilize = Immobilize;
        this.Poison = Poison;
        this.Wound = Wound;
        this.Strengthen = Strengthen;
        this.Muddle = Muddle;
        this.Regenerate = Regenerate;
        this.Invisible = Invisible;
    }

    public StatusDto Stun { get; init; }
    public StatusDto Disarm { get; init; }
    public StatusDto Immobilize { get; init; }
    public StatusDto Poison { get; init; }
    public StatusDto Wound { get; init; }
    public StatusDto Strengthen { get; init; }
    public StatusDto Muddle { get; init; }
    public StatusDto Regenerate { get; init; }
    public StatusDto Invisible { get; init; }

    public void Deconstruct(out StatusDto Stun, out StatusDto Disarm, out StatusDto Immobilize, out StatusDto Poison, out StatusDto Wound, out StatusDto Strengthen, out StatusDto Muddle, out StatusDto Regnerate, out StatusDto Invisible)
    {
        Stun = this.Stun;
        Disarm = this.Disarm;
        Immobilize = this.Immobilize;
        Poison = this.Poison;
        Wound = this.Wound;
        Strengthen = this.Strengthen;
        Muddle = this.Muddle;
        Regnerate = this.Regenerate;
        Invisible = this.Invisible;
    }
}