using AutoMapper;
using GolBet.Entities;
using GolBet.Services.DTOs;

namespace GolBet.Services.Mapping;
public class MappingProfile : Profile
{
    public MappingProfile()
    {
        // Flattening by convention: 
        // MatchDto.HomeTeamName  <- Match.HomeTeam.Name 
        // MatchDto.AwayTeamCrestUrl <- Match.AwayTeam.CrestUrl 

        CreateMap<Match, MatchDto>();
        CreateMap<Match, MatchFormDto>().ReverseMap();

        CreateMap<Team, TeamDto>();
        CreateMap<Team, TeamFormDto>().ReverseMap();

        CreateMap<Match, MatchDetailDto>()
            .ForMember(dto => dto.TotalBets,
                options => options.MapFrom(match => match.Bets.Count));

        CreateMap<Bet, BetDto>()
            .ForMember(dest => dest.HomeTeamName, opt => opt.MapFrom(src => src.Match.HomeTeam.Name))
            .ForMember(dest => dest.AwayTeamName, opt => opt.MapFrom(src => src.Match.AwayTeam.Name))
            .ForMember(dest => dest.HomeTeamCrestUrl, opt => opt.MapFrom(src => src.Match.HomeTeam.CrestUrl))
            .ForMember(dest => dest.AwayTeamCrestUrl, opt => opt.MapFrom(src => src.Match.AwayTeam.CrestUrl))
            .ForMember(dest => dest.MatchStatus, opt => opt.MapFrom(src => src.Match.Status))
            .ForMember(dest => dest.MatchDate, opt => opt.MapFrom(src => src.Match.Date))
            .ForMember(dest => dest.HomeGoals, opt => opt.MapFrom(src => src.Match.HomeGoals))
            .ForMember(dest => dest.AwayGoals, opt => opt.MapFrom(src => src.Match.AwayGoals));
    }
}