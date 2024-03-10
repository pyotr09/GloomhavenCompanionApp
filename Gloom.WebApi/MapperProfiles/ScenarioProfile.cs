using AutoMapper;
using Gloom.Models;
using Gloom.Models.Bosses;
using Gloom.Models.Monsters;
using Gloom.Models.Player_Characters;
using Gloom.Models.Scenario;
using Gloom.WebApi.Models;

namespace Gloom.WebApi.MapperProfiles;

public class ScenarioProfile : Profile
{
    public ScenarioProfile()
    {
        CreateMap<BaseStats, BaseStatsDto>();
        CreateMap<MonsterAbilityCard, MonsterAbilityCardDto>();
        CreateMap<ActionSet, ActionSetDto>();
        CreateMap<MonsterAbilityDeck, MonsterAbilityDeckDto>();
        CreateMap<Character, CharacterDto>();
        CreateMap<Boss, BossDto>();
        CreateMap<Status, StatusDto>();
        CreateMap<Statuses, StatusesDto>();
        CreateMap<Monster, MonsterDto>();
        CreateMap<MonsterGrouping, MonsterGroupDto>();
        CreateMap<Scenario, ScenarioDto>()
            .ForMember(dst => dst.Characters,
                opt => opt.MapFrom(src => src.ParticipantGroups.Where(pg => pg.Type == "Character").Cast<Character>()))
            .ForMember(dst => dst.MonsterGroups,
                opt => opt.MapFrom(src => src.ParticipantGroups.Where(pg => pg.Type == "Monster").Cast<MonsterGrouping>()))
            .ForMember(dst => dst.Bosses,
                opt => opt.MapFrom(src => src.ParticipantGroups.Where(pg => pg.Type == "Boss").Cast<Gloom.Models.Bosses.Boss>()));
    }
}