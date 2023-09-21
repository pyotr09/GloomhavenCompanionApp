using System.Collections.Generic;
using System.Threading.Tasks;
using Gloom.Models;
using Gloom.Models.Monsters;
using Gloom.Models.Scenario;

namespace Gloom.Services;

public interface IScenarioService
{
    Task<Scenario> GetScenarioBySessionIdAsync(int sessionId);
    Task<Scenario> ToggleElementAsync(int sessionId, Element e, bool isWaning);
    Task<Scenario> SetScenarioAsync(int sessionId, int level, int scenarioNumber);
    Task<Scenario> AddMonsterAsync(int sessionId, string monsterName, MonsterTier tier, int monsterNumber);
    Task<Scenario> AddCharacterAsync(int sessionId, string characterName, int characterLevel);
    Task<Scenario> SetCharacterInitiativeAsync(int sessionId, string characterName, int initiative);
    Task<Scenario> UpdateMonsterAsync(int sessionId, string monsterGroup, int monsterNumber, int hp,
        Dictionary<StatusType, bool> statuses);
    Task<Scenario> RemoveMonsterAsync(int sessionId, string monsterGroup, int monsterNumber);
    Task<Scenario> DrawAllMonsterAbilitiesAsync(int sessionId);
    Task<Scenario> DrawMonsterAbilitiesForGroupAsync(int sessionId, string monsterGroup);
    Task<Scenario> EndRoundAsync(int sessionId);
}