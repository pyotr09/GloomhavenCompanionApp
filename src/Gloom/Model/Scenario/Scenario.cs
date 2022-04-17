using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Gloom.Data;
using Gloom.Model.Interfaces;
using Gloom.Model.Monsters;
using Newtonsoft.Json.Linq;

namespace Gloom.Model.Scenario;

public class Scenario
{
    public Scenario(int level, string name)
    {
        _level = level;
        _name = name;
        Participants = new List<IScenarioParticipantGroup>(); 
    }

    public Scenario(int level, int number, string expansion)
    {
        _level = level;
        using var r = new StreamReader("ScenarioInformation.json");
        var jsonString = r.ReadToEnd();
        var scenarios = JArray.Parse(jsonString);

        var scenarioToken = scenarios.SelectToken($"$[?(@.Number == {number} && @.Expansion == '{expansion}')]");
        _name = (string) scenarioToken["Name"];
        var monsterListToken = scenarioToken.SelectToken("MonsterList");
        
        Participants = new List<IScenarioParticipantGroup>();
        foreach (var monsterToken in monsterListToken)
        {
            var monsterName = (string) monsterToken;
            AddMonsterGroup(monsterName, GetDeckName(monsterName, expansion));
        }
        
    }

    private string GetDeckName(string monsterName, string expansion)
    {
        if (expansion == "Jaws of the Lion")
        {
            if (monsterName.Contains("Imp"))
                return "Jaws of the Lion Imp";
            if (monsterName.Equals("Monstrosity"))
                return "Monstrosity";
            if (monsterName.Equals("Vermling Raider"))
                return "Vermling Raider";
            if (monsterName.Equals("Vermling Scout"))
                return "Vermling Scout";
            if (monsterName.Equals("Zealot"))
                return "Zealot";
            if (monsterName.Equals("Blood Ooze"))
                return "Blood Ooze";
            if (MonsterStatsDeserialized.Instance.Bosses.Any(b => b.Name == monsterName))
                return "Jaws of the Lion Boss";
            return "Jaws of the Lion " + monsterName;
        }
        if (expansion == "Crimson Scales")
        {
            if (monsterName.Equals("Toxic Imp"))
                return "Toxic Imp";
        }
        if (monsterName.Contains("Archer"))
            return "Archer";
        if (monsterName.Contains("Guard"))
            return "Guard";
        if (monsterName.Contains("Imp"))
            return "Imp";
        if (monsterName.Contains("Scout"))
            return "Scout";
        if (monsterName.Contains("Shaman"))
            return "Shaman";
        if (monsterName.Contains("Ashblade"))
            return "Ashblade";
        if (monsterName.Contains("Savage"))
            return "Savage";
        if (monsterName.Contains("Tracker"))
            return "Tracker";
        if (MonsterStatsDeserialized.Instance.Bosses.Any(b => b.Name == monsterName))
            return "Boss";

        return monsterName;
    }

    private int _level;
    private string _name;
    public List<IScenarioParticipantGroup> Participants;

    public void AddMonsterGroup(string monsterName, string deckName)
    {
        var monsterType = new MonsterType(monsterName, deckName)
        {
            MaxNumberOnBoard = 6 // need data for this
        };
        Participants.Add(new MonsterGrouping(monsterType, _level));
    }

    public void EndRound()
    {
        
    }

    public void Draw()
    {
        Participants.ForEach(g => g.Draw());
    }

    public override string ToString()
    {
        var s = new StringBuilder()
            .AppendLine(new string('-' , 100))
            .Append("  ").Append(_name).Append(' ', 82 - _name.Length).Append($"Level: {_level}").AppendLine()
            .AppendLine(new string('-' , 100));
        foreach (var group in Participants.OrderBy(g => g.Initiative))
        {
            s.Append(group);
        }

        s.AppendLine(new string('-', 100));
        return s.ToString();
    }
}