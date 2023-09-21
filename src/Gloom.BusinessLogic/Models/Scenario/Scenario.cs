using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Gloom.Data;
using Gloom.Models.Bosses;
using Gloom.Models.Interfaces;
using Gloom.Models.Monsters;
using Gloom.Models.Player_Characters;
using Newtonsoft.Json.Linq;
using Boss = Gloom.Models.Bosses.Boss;

namespace Gloom.Models.Scenario;

public class Scenario
{
    public Scenario()
    {
        
    }
    public Scenario(int level, string name)
    {
        Level = level;
        Name = name;
        MonsterGroups = new List<IScenarioParticipantGroup>(); 
    }

    public Scenario(int level, int number, string expansion, List<Tuple<string, int>> charNameAndLevels = null)
    {
        Level = level;
        using var r = new StreamReader("Data/ScenarioInformation.json");
        var jsonString = r.ReadToEnd();
        var scenarios = JArray.Parse(jsonString);

        var scenarioToken = scenarios.SelectToken($"$[?(@.Number == {number} && @.Expansion == '{expansion}')]");
        Name = (string) scenarioToken["Name"];
        var monsterListToken = scenarioToken.SelectToken("MonsterList");
        
        MonsterGroups = new List<IScenarioParticipantGroup>();
        foreach (var monsterToken in monsterListToken)
        {
            var monsterName = (string) monsterToken;
            if (MonsterStatsDeserialized.Instance.Bosses.Any(b => b.Name == monsterName))
            {
                AddBoss(monsterName);
            }
            else
            {
                AddMonsterGroup(monsterName, Utils.GetDeckName(monsterName, expansion));
            }
        }

        if (charNameAndLevels != null)
        {
            foreach (var charNameAndLevel in charNameAndLevels)
            {
                AddCharacter(charNameAndLevel.Item1, charNameAndLevel.Item2);
            }
        }

        Elements = new Dictionary<Element, int>()
        {
            {Element.Fire, 0},
            {Element.Ice, 0},
            {Element.Earth, 0},
            {Element.Air, 0},
            {Element.Light, 0},
            {Element.Dark, 0},
        };

        IsBetweenRounds = true;
    }
    
    public int Level;
    public string Name;
    public int NumCharacters = 4;
    public List<IScenarioParticipantGroup> MonsterGroups;
    public bool IsBetweenRounds;
    public Dictionary<Element, int> Elements;

    public void AddMonsterGroup(string monsterName, string deckName)
    {
        var monsterType = new MonsterType(monsterName, deckName)
        {
        };
        MonsterGroups.Add(new MonsterGrouping(monsterType, Level));
    }

    public void AddBoss(string bossName)
    {
        var bossStats = new BossStats(bossName, NumCharacters);
        var bossType = new BossType(bossName, bossStats);
        MonsterGroups.Add(new Boss(bossType, Level, NumCharacters));
    }

    public void AddCharacter(string characterName, int charLevel)
    {
        MonsterGroups.Add(new Character(characterName, charLevel));
    }

    public void AddMonster(string monsterGroupName, MonsterTier tier, int number = -1)
    {
        var monsterGrouping = (MonsterGrouping)
            MonsterGroups.First(g => 
                g.Type == "Monster" && g.Name == monsterGroupName);
        monsterGrouping.AddMonster(tier, Level, number);
    }

    public void RemoveMonster(string monsterGroupName, int number)
    {
        var monsterGrouping = (MonsterGrouping)
            MonsterGroups.First(g => 
                g.Type == "Monster" && g.Name == monsterGroupName);
        monsterGrouping.RemoveMonster(number);
    }

    public void EndRound()
    {
        MonsterGroups.ForEach(g => g.RefreshForEndOfRound());
        foreach (var e in Elements.Keys)
        {
            if (Elements[e] > 0)
                Elements[e]--;
        }
        IsBetweenRounds = true;
    }

    public void InfuseElement(Element e)
    {
        Elements[e] = 2;
    }
    
    public void ConsumeElement(Element e)
    {
        Elements[e] = 0;
    }
    
    public void SetElementWaning(Element e)
    {
        Elements[e] = 1;
    }

    public void Draw()
    {
        MonsterGroups.ForEach(g => g.Draw());
        IsBetweenRounds = false;
    }

    public override string ToString()
    {
        var s = new StringBuilder()
            .AppendLine(new string('-' , 100))
            .Append("  ").Append(Name).Append(' ', 82 - Name.Length).Append($"Level: {Level}").AppendLine()
            .AppendLine(new string('-' , 100));
        foreach (var group in MonsterGroups.OrderBy(g => g.Initiative))
        {
            s.Append(group);
        }

        s.AppendLine(new string('-', 100));
        return s.ToString();
    }
}