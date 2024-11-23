using Levi9_competition.Models;

namespace Levi9_competition.Interfaces
{
    public interface IPlayerRepo
    {
        Task<Player> CreateAsync(Player playerModel);
        Task<Player?> GetByIdAsync(string id);
        Task<bool> PlayerExisist(string nickname);
        Task<List<Player>> GetAllAsync();
        Task<Player> UpdateAsync(Player playerModel);
        Task UpdateRangeAsync(IEnumerable<Player> players);
    }
}
