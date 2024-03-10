namespace Gloom.WebApi.Models;

public class MonsterAbilityCardDto
{
    public int Initiative;
    public string Name;
    public string Expansion;
    public List<ActionSetDto> Actions;
    public bool ShuffleAfter;
    public string ImagePath;
}

public class ActionSetDto
{
    public string BaseActionText;
    public string NormalActionText;
    public string EliteActionText;
}