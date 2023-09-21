using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.Model;
using Gloom.Models;
using Gloom.Models.Bosses;
using Gloom.Models.Monsters;
using Gloom.Models.Player_Characters;
using Gloom.Models.Scenario;
using Newtonsoft.Json;

namespace Gloom.Services.Implementation;

public class ScenarioService : IScenarioService
{
    private readonly AmazonDynamoDBClient _dbClient;
    public ScenarioService(AmazonDynamoDBClient dbClient)
    {
        _dbClient = dbClient;
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

    public async Task<Scenario> AddMonsterAsync(int sessionId, string monsterName, MonsterTier tier, int monsterNumber)
    {
        var scenario = await GetDbScenarioAsync(sessionId);
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
        var character = (Character) scenario.MonsterGroups.First(mg => mg.Name == characterName);
        character.Initiative = initiative;
        await SaveScenarioAsync(sessionId, scenario);
        return scenario;
    }

    public async Task<Scenario> UpdateMonsterAsync(int sessionId, string monsterGroup, int monsterNumber,
        int hp, Dictionary<StatusType, bool> statuses)
    {
        var scenario = await GetDbScenarioAsync(sessionId);
        var monster = (scenario.MonsterGroups.First(g => g.Name == monsterGroup)
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
        var group = scenario.MonsterGroups.First(g => g.Name == monsterGroup);
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
        var getRequest = new GetItemRequest
        {
            TableName = "GloomAppSessions",
            Key = new Dictionary<string, AttributeValue> {{"Id", new AttributeValue {N = sessionId.ToString()}}},
            ProjectionExpression = "Id, Scenario"
        };
        var response = await _dbClient.GetItemAsync(getRequest);
        if (!response.IsItemSet)
        {
            return null;
        }
        var scenarioString = response.Item["Scenario"].S;
        return JsonConvert.DeserializeObject<Scenario>(scenarioString, new JsonSerializerSettings
        {
            TypeNameHandling = TypeNameHandling.Auto
        });
    }
    
    private async Task SaveScenarioAsync(int sessionId, Scenario scenario)
    {
        // update -- puts if doesn't already exist
        var updateRequest = new UpdateItemRequest
        {
            TableName = "GloomAppSessions",
            Key = new Dictionary<string, AttributeValue>
            {
                {"Id", new AttributeValue {N = sessionId.ToString()}}
            },
            ExpressionAttributeNames = new Dictionary<string, string>()
            {
                {"#S", "Scenario"}
            },
            ExpressionAttributeValues = new Dictionary<string, AttributeValue>()
            {
                {":scenario", new AttributeValue {S = JsonConvert.SerializeObject(scenario, new JsonSerializerSettings
                    {
                        TypeNameHandling = TypeNameHandling.Auto
                    })}
                }
            },
            UpdateExpression = "SET #S = :scenario"
        };
        await _dbClient.UpdateItemAsync(updateRequest);
    }
}