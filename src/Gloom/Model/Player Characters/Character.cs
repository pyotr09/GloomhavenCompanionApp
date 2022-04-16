namespace Gloom.Model.Player_Characters;

public class Character
{
    public Character(int maxHealth, int level, string name)
    {
        MaxHealth = maxHealth;
        CurrentHealth = MaxHealth;
        Level = level;
        Name = name;
        XP = 0;
        Gold = 0;
        Initiative = null;

    }

    public int CurrentHealth;
    public int MaxHealth;
    public int Level;
    public string Name;
    public int XP;
    public int Gold;
    public int? Initiative;

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
    public void IncreaseXPBy(int change)
    {
            XP += change;
    }

    public void DecreaseXPBy(int change)
    {
        if (change > XP)
        {
            XP = 0;
        }
        else
        {
            XP -= change;
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
}