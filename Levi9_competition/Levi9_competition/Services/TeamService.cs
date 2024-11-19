using Levi9_competition.Dtos.Team;
using Levi9_competition.Interfaces;
using Levi9_competition.Mappers;
using Levi9_competition.Models;

namespace Levi9_competition.Services
{
    public class TeamService
    {
        private readonly ITeamRepo _teamRepo;

        public TeamService(ITeamRepo teamRepo)
        {
            _teamRepo = teamRepo;
        }

        public async Task<Team> CreateTeamAsync(CreateTeamRequestDto teamDto)
        {
            if (await _teamRepo.TeamExisist(teamDto.TeamName))
            {
                throw new ArgumentException("Team name is already taken.");
            }

            var playerIds = teamDto.Players.ToList();

            var playersInDb = await _teamRepo.GetPlayersByGuidsAsync(playerIds);

            if (playersInDb.Count != 5)
            {
                throw new ArgumentException("One or more players do not exist in the database.");
            }

            foreach (var player in playersInDb)
            {
                if(player.Team != null)
                {
                    throw new ArgumentException("One or more players are already in another team.");
                }
            }

            var team = teamDto.ToTeamFromCreateDTO();
            team.Players = playersInDb;
            foreach (var player in playersInDb)
            {
                player.Team = team.Id;
                player.RatingAdjustment = 50;
            }

            await _teamRepo.CreateAsync(team);
            return team;
        }

        public async Task<Team?> GetTeamByIdAsync(string id)
        {
            return await _teamRepo.GetByIdAsync(id);
        }
    }
}
