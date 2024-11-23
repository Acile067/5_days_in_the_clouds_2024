using Levi9_competition.Interfaces;
using Levi9_competition.Models;

namespace Levi9_competition.Services
{
    public class PlayerService
    {
        private readonly ITeamRepo _teamRepo;
        private readonly IMatchRepo _matchRepo;
        private readonly IPlayerRepo _playerRepo;
        public PlayerService(ITeamRepo teamRepo, IMatchRepo matchRepo, IPlayerRepo playerRepo)
        {
            _teamRepo = teamRepo;
            _matchRepo = matchRepo;
            _playerRepo = playerRepo;
        }
        public async Task DeleteAllDataAsync()
        {
            await _playerRepo.DeleteAllAsync();
            await _teamRepo.DeleteAllAsync();
            await _matchRepo.DeleteAllAsync();
        }

        public async Task<List<Player>> GetAllAsync()
        {
            return await _playerRepo.GetAllAsync();
        }

        public async Task<Player> GetByIdAsync(string id)
        {
            return await _playerRepo.GetByIdAsync(id);
        }

        public async Task<bool> PlayerExisist(string nickname)
        {
            return await _playerRepo.PlayerExisist(nickname);
        }

        public async Task CreateAsync(Player playerModel)
        {
            await _playerRepo.CreateAsync(playerModel);
        }

        public async Task UpdateAsync(Player player)
        {
            await _playerRepo.UpdateAsync(player);
        }
    }
}
