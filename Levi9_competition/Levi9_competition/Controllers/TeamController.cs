using Levi9_competition.Dtos.Team;
using Levi9_competition.Interfaces;
using Levi9_competition.Mappers;
using Levi9_competition.Models;
using Levi9_competition.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Levi9_competition.Controllers
{
    [Route("teams")]
    [ApiController]
    public class TeamController : ControllerBase
    {
        private readonly TeamService _teamService;
        private readonly IPlayerRepo _playerRepo;

        public TeamController(TeamService teamService, IPlayerRepo playerRepo)
        {
            _teamService = teamService;
            _playerRepo = playerRepo;
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateTeamRequestDto teamDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                var team = await _teamService.CreateTeamAsync(teamDto);
                return Ok(team.ToTeamDto());
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpGet]
        [Route("{id}")]
        public async Task<IActionResult> GetById([FromRoute] string id)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var team = await _teamService.GetTeamByIdAsync(id);
            if (team == null)
                return NotFound();

            return Ok(team.ToTeamDto());
        }

        [HttpPost]
        [Route("generate_teams")]
        public async Task<IActionResult> GenerateTeams([FromQuery] int teamSize)
        {
            if (teamSize <= 0)
            {
                return BadRequest("Team size must be greater than zero.");
            }

            try
            {
                // Fetch players sorted by ELO in descending order
                var players = await _playerRepo.GetAllAsync();

                if (players == null || players.Count < 2 * teamSize)
                {
                    return BadRequest("Not enough players to form two teams.");
                }

                // Select the first 2 * teamSize players without a team (sorted by ELO)
                var eligiblePlayers = players
                    .Where(p => p.Team == null) // Assuming TeamId is null for players without a team
                    .OrderByDescending(p => p.Elo)
                    .Take(2 * teamSize)
                    .ToList();

                if (eligiblePlayers.Count < 2 * teamSize)
                {
                    return BadRequest("Not enough eligible players to form two teams.");
                }

                // Create two teams
                var team1Players = new List<Player>();
                var team2Players = new List<Player>();

                if (teamSize == 1)
                {
                    // Special case: Team size of 1
                    team1Players.Add(eligiblePlayers[0]); // Add first player to team1
                    team2Players.Add(eligiblePlayers[1]); // Add second player to team2
                }
                else if(teamSize % 2 == 0)
                {
                    for (int i = 0; i < teamSize / 2; i++)
                    {
                        // First team gets the first and last players
                        team1Players.Add(eligiblePlayers[i]);
                        team1Players.Add(eligiblePlayers[2 * teamSize - i - 1]);

                        // Second team gets the next two players
                        if (i + 1 < teamSize)
                        {
                            team2Players.Add(eligiblePlayers[i + 1]);
                            team2Players.Add(eligiblePlayers[2 * teamSize - i - 2]);
                        }
                    }
                }
                else if(teamSize % 2 == 1)
                {
                    for (int i = 0; i < teamSize / 2; i++)
                    {
                        // First team gets the first and last players
                        team1Players.Add(eligiblePlayers[i]);
                        team2Players.Add(eligiblePlayers[2 * teamSize - i - 1]);

                        // Second team gets the next two players
                        if (i + 1 < teamSize)
                        {
                            team1Players.Add(eligiblePlayers[i + 1]);
                            team2Players.Add(eligiblePlayers[2 * teamSize - i - 2]);
                        }
                    }

                    // After distributing half, handle the middle players for odd team sizes
                    team1Players.Add(eligiblePlayers[teamSize - 1]);  // Add the N-th player to team1
                    team2Players.Add(eligiblePlayers[teamSize]);
                }

                Team team1 = new Team
                {
                    Id = Guid.NewGuid().ToString(),
                    TeamName = Guid.NewGuid().ToString(),
                    Players = team1Players
                };

                Team team2 = new Team
                {
                    Id = Guid.NewGuid().ToString(),
                    TeamName = Guid.NewGuid().ToString(),
                    Players = team2Players
                };

                foreach (var player in team1Players)
                {
                    player.Team = team1.Id;
                }

                foreach (var player in team2Players)
                {
                    player.Team = team2.Id;
                }

                // Save team assignments to the database
                await _playerRepo.UpdateRangeAsync(eligiblePlayers);

                // Return the formed teams
                return Ok(new
                {
                    Team1 = team1,
                    Team2 = team2
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, "An error occurred while generating teams.");
            }
        }
    }
}
