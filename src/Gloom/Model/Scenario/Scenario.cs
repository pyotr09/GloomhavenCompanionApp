using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Gloom.Data;
using Gloom.Model.Interfaces;
using Gloom.Model.Monsters;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

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
        MonsterGroups = new List<MonsterGrouping>(); 
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
        
        MonsterGroups = new List<MonsterGrouping>();
        foreach (var monsterToken in monsterListToken)
        {
            var monsterName = (string) monsterToken;
            AddMonsterGroup(monsterName, Utils.GetDeckName(monsterName, expansion));
        }
        
    }
    
    public int Level;
    public string Name; 
    public List<MonsterGrouping> MonsterGroups;
    public string Text => ToString();

    public void AddMonsterGroup(string monsterName, string deckName)
    {
        var monsterType = new MonsterType(monsterName, deckName)
        {
            MaxNumberOnBoard = 6 // need data for this
        };
        MonsterGroups.Add(new MonsterGrouping(monsterType, Level));
    }

    public void AddMonster(string monsterGroupName, MonsterTier tier, int number = -1)
    {
        var monsterGrouping =
            MonsterGroups.First(g => g.Name == monsterGroupName);
        monsterGrouping.AddMonster(tier, number);
    }

    public void EndRound()
    {
        
    }

    public void Draw()
    {
        MonsterGroups.ForEach(g => g.Draw());
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