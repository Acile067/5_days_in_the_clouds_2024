using Levi9_competition.Models;

namespace Levi9_competition.Interfaces
{
    public interface ITeamRepo
    {
        Task<Team> CreateAsync(Team teamModel);
        Task<Team?> GetByIdAsync(string id);
        Task<List<Player>> GetPlayersByGuidsAsync(List<string> playerIds);
        Task<Team> UpdateAsync(Team teamModel);
    }
}
