using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.DataModel;
using Amazon.DynamoDBv2.Model;
using Gloom.Common;
using Gloom.Data.DynamoDbTables;
using Gloom.Models;
using Gloom.Models.Bosses;
using Gloom.Models.Monsters;
using Gloom.Models.Player_Characters;
using Gloom.Models.Scenario;
using Newtonsoft.Json;
using Utils = Gloom.Common.Utils;

namespace Gloom.Services.Implementation;

public class ScenarioService : IScenarioService
{
    private readonly IDynamoDBContext _dynamoDbContext;
    public ScenarioService(IDynamoDBContext dynamoDbContext)
    {
        _dynamoDbContext = dynamoDbContext;
    }

    public async Task<int> StartNewSessionAsync()
    {
        var newSession = new GloomAppSessions
        {
            Id = Utils.GenerateId(),
            Scenario = "Scenario not yet set"
        };
        await _dynamoDbContext.SaveAsync(newSession);
        return newSession.Id;
    }

    public async Task<Scenario> GetScenarioBySessionIdAsync(int sessionId)
    {
        return await GetDbScenarioAsync(sessionId);
    }

    public async Task<Scenario> ToggleElementAsync(int sessionId, Element e, bool isWaning)
    {
        var scenario = await GetDbScenarioAsync(sessionId);
        if (isWaning)
        {
            scenario.SetElementWaning(e);
        }
        else
        {
            if (scenario.Elements[e] > 0)
            {
                scenario.ConsumeElement(e);
            }
            else
            {
                scenario.InfuseElement(e);
            }
        }

        await SaveScenarioAsync(sessionId, scenario);
        return scenario;
    }

    public async Task<Scenario> SetScenarioAsync(int sessionId, int level, int scenarioNumber)
    {
        var scenario = new Scenario(level, scenarioNumber, "Gloomhaven");
        await SaveScenarioAsync(sessionId, scenario);
        return scenario;
    }

    public async Task<Scenario> AddMonsterAsync(int sessionId, string monsterName, string tierString, int monsterNumber)
    {
        var scenario = await GetDbScenarioAsync(sessionId);
        var tier = tierString == "elite" ? MonsterTier.Elite : MonsterTier.Normal;
        scenario.AddMonster(monsterName, tier, monsterNumber);
        await SaveScenarioAsync(sessionId, scenario);
        return scenario;
    }

    public async Task<Scenario> AddCharacterAsync(int sessionId, string characterName, int characterLevel)
    {
        var scenario = await GetDbScenarioAsync(sessionId);
        scenario.AddCharacter(characterName, characterLevel);
        await SaveScenarioAsync(sessionId, scenario);
        return scenario;
    }

    public async Task<Scenario> SetCharacterInitiativeAsync(int sessionId, string characterName, int initiative)
    {
        var scenario = await GetDbScenarioAsync(sessionId);
        var character = (Character) scenario.ParticipantGroups.First(mg => mg.Name == characterName);
        character.Initiative = initiative;
        await SaveScenarioAsync(sessionId, scenario);
        return scenario;
    }

    public async Task<Scenario> UpdateMonsterAsync(int sessionId, string monsterGroup, int monsterNumber,
        int hp, Dictionary<StatusType, bool> statuses)
    {
        var scenario = await GetDbScenarioAsync(sessionId);
        var monster = (scenario.ParticipantGroups.First(g => g.Name == monsterGroup)
                as MonsterGrouping)?
            .Monsters.First(m => m.MonsterNumber == monsterNumber);
        if (monster != null)
        {
            monster.CurrentHitPoints = hp;
            foreach (var statusKvp in statuses)
            {
                monster.Statuses.SetStatus(statusKvp.Key, statusKvp.Value, true);
            }
        }
        await SaveScenarioAsync(sessionId, scenario);
        return scenario;
    }

    public async Task<Scenario> RemoveMonsterAsync(int sessionId, string monsterGroup, int monsterNumber)
    {
        var scenario = await GetDbScenarioAsync(sessionId);
        scenario.RemoveMonster(monsterGroup, monsterNumber);
        await SaveScenarioAsync(sessionId, scenario);
        return scenario;
    }

    public async Task<Scenario> DrawAllMonsterAbilitiesAsync(int sessionId)
    {
        var scenario = await GetDbScenarioAsync(sessionId);
        scenario.Draw();
        await SaveScenarioAsync(sessionId, scenario);
        return scenario;
    }

    public async Task<Scenario> DrawMonsterAbilitiesForGroupAsync(int sessionId, string monsterGroup)
    {
        var scenario = await GetDbScenarioAsync(sessionId);
        var group = scenario.ParticipantGroups.First(g => g.Name == monsterGroup);
        if (group is Boss boss)
        {
            boss.Activate();
            if (!scenario.IsBetweenRounds)
                group.Draw();
        }
        else
        {
            group.Draw();
        }

        await SaveScenarioAsync(sessionId, scenario);
        return scenario;
    }

    public async Task<Scenario> EndRoundAsync(int sessionId)
    {
        var scenario = await GetDbScenarioAsync(sessionId);
        scenario.EndRound();
        await SaveScenarioAsync(sessionId, scenario);
        return scenario;
    }
    
    private async Task<Scenario> GetDbScenarioAsync(int sessionId)
    {
        var session = await _dynamoDbContext.LoadAsync<GloomAppSessions>(sessionId);
        if (session == null)
        {
            return null;
        }
        var scenarioString = session.Scenario;

        try
        {
            var scenario = JsonConvert.DeserializeObject<Scenario>(scenarioString, new JsonSerializerSettings
            {
                TypeNameHandling = TypeNameHandling.Auto
            });

            return scenario;
        }
        catch (Exception e)
        {
            return null;
        }
    }
    
    private async Task SaveScenarioAsync(int sessionId, Scenario scenario)
    {
        // save -- puts if doesn't already exist
        var scenarioSerialized = JsonConvert.SerializeObject(scenario, new JsonSerializerSettings
        {
            TypeNameHandling = TypeNameHandling.Auto
        });
        await _dynamoDbContext.SaveAsync(new GloomAppSessions {Id = sessionId, Scenario = scenarioSerialized});
    }
}