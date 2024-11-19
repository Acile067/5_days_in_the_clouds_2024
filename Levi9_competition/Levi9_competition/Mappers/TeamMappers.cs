using Levi9_competition.Dtos.Player;
using Levi9_competition.Dtos.Team;
using Levi9_competition.Models;

namespace Levi9_competition.Mappers
{
    public static class TeamMappers
    {
        public static TeamDto ToTeamDto(this Team teamModel)
        {
            return new TeamDto
            {
                Id = teamModel.Id,
                TeamName = teamModel.TeamName,
                Players = teamModel.Players.Select(p => new PlayerDto
                {
                    Id = p.Id,
                    Nickname = p.Nickname,
                    Wins = p.Wins,
                    Losses = p.Losses,
                    Elo = p.Elo,
                    HoursPlayed = p.HoursPlayed,
                    TeamId = p.Team, 
                    RatingAdjustment = p.RatingAdjustment
                }).ToList()
            };
        }

        public static Team ToTeamFromCreateDTO(this CreateTeamRequestDto teamModel)
        {
            return new Team
            {
                Id = Guid.NewGuid().ToString(),
                TeamName = teamModel.TeamName,
                Players = new List<Player>()
            };
        }
    }
}
