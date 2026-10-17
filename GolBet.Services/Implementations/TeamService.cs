using AutoMapper;
using GolBet.Entities;
using GolBet.Repositories.Interfaces;
using GolBet.Services.DTOs;
using GolBet.Services.Interfaces;

namespace GolBet.Services.Implementations;

public class TeamService : ITeamService
{
    private readonly IGenericRepository<Team> _teamRepository;
    private readonly IMapper _mapper;

    public TeamService(IGenericRepository<Team> teamRepository, IMapper mapper)
    {
        _teamRepository = teamRepository;
        _mapper = mapper;
    }

    public async Task<IEnumerable<TeamDto>> GetAllAsync(string? searchTerm = null)
    {
        var teams = await _teamRepository.GetAllAsync();

        if (!string.IsNullOrWhiteSpace(searchTerm))
        {
            var term = searchTerm.Trim();
            teams = teams.Where(t =>
                t.Name.Contains(term, StringComparison.OrdinalIgnoreCase) ||
                (!string.IsNullOrEmpty(t.City) && t.City.Contains(term, StringComparison.OrdinalIgnoreCase)));
        }

        return _mapper.Map<IEnumerable<TeamDto>>(teams);
    }

    public async Task<TeamFormDto?> GetForEditAsync(int id)
    {
        var team = await _teamRepository.GetByIdAsync(id);
        return team is null ? null : _mapper.Map<TeamFormDto>(team);
    }

    public async Task CreateAsync(TeamFormDto dto)
    {
        var team = _mapper.Map<Team>(dto);
        await _teamRepository.AddAsync(team);
    }

    public async Task UpdateAsync(TeamFormDto dto)
    {
        var team = await _teamRepository.GetByIdAsync(dto.Id)
            ?? throw new KeyNotFoundException($"No se encontró el equipo con ID {dto.Id}");

        _mapper.Map(dto, team);
        await _teamRepository.UpdateAsync(team);
    }

    public async Task DeactivateAsync(int id)
    {
        await _teamRepository.DeactivateAsync(id);
    }
}
