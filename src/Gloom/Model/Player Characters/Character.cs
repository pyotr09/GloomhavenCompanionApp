using System;
using System.Collections.Generic;
using Gloom.Data;
using Gloom.Model.Interfaces;
using Gloom.Model.Monsters;

namespace Gloom.Model.Player_Characters;

public class Character : IScenarioParticipantGroup
{
    public Character(string name, int level)
    {
        Name = name;
        Level = level;
        MaxHealth = CurrentHealth = CharacterData.HPbyLevel[name][level];
        Xp = 0;
        Gold = 0;
        Initiative = null;
    }

    public Character()
    {
    }

    public int CurrentHealth { get; set; }
    public int MaxHealth { get; set; }
    public int Level { get; set; }
    public string Name { get; set; }
    public int Xp { get; set; }
    public int Gold { get; set; }
    public int? Initiative { get; set; }
    public string Type => "Character";
    public string DeckName => "";
    public List<BaseStats> BaseStatsList => new();
    public MonsterAbilityDeck AbilityDeck => null;

    public void IncreaseCurrentHealthBy(int change)
    {
        if (change + CurrentHealth > MaxHealth)
        {
            CurrentHealth = MaxHealth;
        }
        else
        {
            CurrentHealth += change;
        }
    }

    public void DecreaseCurrentHealthBy(int change)
    {
        if (change > CurrentHealth)
        {
            CurrentHealth = 0;
        }
        else
        {
            CurrentHealth -= change;
        }
    }
    public void IncreaseXpBy(int change)
    {
            Xp += change;
    }

    public void DecreaseXpBy(int change)
    {
        if (change > Xp)
        {
            Xp = 0;
        }
        else
        {
            Xp -= change;
        }

    }

    public void IncreaseGoldBy(int change)
    {
        Gold += change;
    }

    public void DecreaseGoldBy(int change)
    {
        if (change > Gold)
        {
            Gold = 0;
        }
        else
        {
            Gold -= change;
        }

    }

    public void SetInitiative(int initiative) 
    {
        Initiative = initiative;
    }

    public void Draw()
    {
        // no ability deck to draw
    }

    public void RefreshForEndOfRound()
    {
        Initiative = null;
    }
}