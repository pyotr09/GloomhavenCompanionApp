using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Amazon.Lambda.Core;
using Gloom.Data;
using Gloom.Model.Bosses;
using Gloom.Model.Interfaces;
using Gloom.Model.Monsters;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Boss = Gloom.Model.Bosses.Boss;

namespace Gloom.Model.Scenario;

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

    public Scenario(int level, int number, string expansion)
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

        IsBetweenRounds = true;
    }
    
    public int Level;
    public string Name;
    public int NumCharacters = 4;
    public List<IScenarioParticipantGroup> MonsterGroups;
    public bool IsBetweenRounds;

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

    public void AddMonster(string monsterGroupName, MonsterTier tier, int number = -1)
    {
        var monsterGrouping = (MonsterGrouping)
            MonsterGroups.First(g => 
                g.Type == "Monster" && g.Name == monsterGroupName);
        monsterGrouping.AddMonster(tier, number);
    }

    public void EndRound()
    {
        MonsterGroups.ForEach(g => g.RefreshForEndOfRound());
        IsBetweenRounds = true;
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