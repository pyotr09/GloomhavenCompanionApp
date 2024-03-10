using AutoMapper;
using Gloom.Services;
using Gloom.WebApi.MapperProfiles;
using Gloom.WebApi.Models;
using Microsoft.AspNetCore.Mvc;

namespace Gloom.WebApi.Controllers;

[Route("scenario")]
[ApiController]
public class ScenarioController : ControllerBase
{
    private readonly IScenarioService _scenarioService;
    private readonly Mapper _mapper;
    public ScenarioController(IScenarioService scenarioService)
    {
        _scenarioService = scenarioService;

        var mapConfig = new MapperConfiguration(cfg =>
        {
            cfg.AddProfile<ScenarioProfile>();
        });
        _mapper = new Mapper(mapConfig);
    }

    [HttpGet("{sessionId}")]
    public async Task<ScenarioDto> GetScenario(int sessionId)
    {
        var scenario = await _scenarioService.GetScenarioBySessionIdAsync(sessionId);
        return _mapper.Map<ScenarioDto>(scenario);
    }
    
    [HttpPost("new")]
    public async Task<int> StartNewSession()
    {
        var sessionId = await _scenarioService.StartNewSessionAsync();
        return sessionId;
    }

    [HttpPost("{sessionId}/set")]
    public async Task<ScenarioDto> SetScenario(int sessionId, SetScenarioDto dto)
    {
        var scenario = await _scenarioService.SetScenarioAsync(sessionId, dto.Level, dto.Number);
        return _mapper.Map<ScenarioDto>(scenario);
    }

    [HttpPost("{sessionId}/addMonster")]
    public async Task<ScenarioDto> AddMonster(int sessionId, AddMonsterDto dto)
    {
        var scenario = await _scenarioService.AddMonsterAsync(sessionId, dto.GroupName, dto.Tier,
            dto.Number);
        return _mapper.Map<ScenarioDto>(scenario);
    }

    [HttpPost("{sessionId}/addCharacter")]
    public async Task<ScenarioDto> AddCharacter(int sessionId, AddCharacterDto dto)
    {
        var scenario = await _scenarioService.AddCharacterAsync(sessionId, dto.Name, dto.Level);
        return _mapper.Map<ScenarioDto>(scenario);
    }

    [HttpPost("{sessionId}/setInitiative")]
    public async Task<ScenarioDto> SetInitiative(int sessionId, SetInitiativeDto dto)
    {
        var scenario = await _scenarioService.SetCharacterInitiativeAsync(sessionId, dto.Name, dto.Initative);
        return _mapper.Map<ScenarioDto>(scenario);
    }

    [HttpPost("{sessionId}/updateMonsterState")]
    public async Task<ScenarioDto> UpdateMonsterState(int sessionId, UpdateMonsterStateDto dto)
    {
        var scenario = await _scenarioService.UpdateMonsterAsync(sessionId,
            dto.GroupName, dto.MonsterNumber, dto.NewHp, dto.Statuses);
        return _mapper.Map<ScenarioDto>(scenario);
    }

    [HttpPost("{sessionId}/removeMonster")]
    public async Task<ScenarioDto> RemoveMonster(int sessionId, RemoveMonsterDto dto)
    {
        var scenario = await _scenarioService.RemoveMonsterAsync(sessionId, dto.GroupName, dto.Number);
        return _mapper.Map<ScenarioDto>(scenario);
    }

    [HttpPost("{sessionId}/drawAbility")]
    public async Task<ScenarioDto> DrawAbility(int sessionId)
    {
        var scenario = await _scenarioService.DrawAllMonsterAbilitiesAsync(sessionId);
        return _mapper.Map<ScenarioDto>(scenario);
    }

    [HttpPost("{sessionId}/drawForGroup")]
    public async Task<ScenarioDto> DrawAbilityForGroup(int sessionId, DrawAbilityForGroupDto dto)
    {
        var scenario = await _scenarioService.DrawMonsterAbilitiesForGroupAsync(sessionId, dto.GroupName);
        return _mapper.Map<ScenarioDto>(scenario);
    }

    [HttpPost("{sessionId}/endRound")]
    public async Task<ScenarioDto> EndRound(int sessionId)
    {
        var scenario = await _scenarioService.EndRoundAsync(sessionId);
        return _mapper.Map<ScenarioDto>(scenario);
    }
}