using Levi9_competition.Interfaces;

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

    }
}
