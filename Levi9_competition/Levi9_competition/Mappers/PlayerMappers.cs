using Levi9_competition.Dtos.Player;
using Levi9_competition.Models;
using System.Runtime.CompilerServices;

namespace Levi9_competition.Mappers
{
    public static class PlayerMappers
    {
        public static PlayerDto ToPlayerDto(this Player playerModel)
        {
            return new PlayerDto 
            { 
                Id = playerModel.Id,
                Nickname = playerModel.Nickname,
                Wins = playerModel.Wins,
                Losses = playerModel.Losses,
                Elo = playerModel.Elo,
                HoursPlayed = playerModel.HoursPlayed,
                Team = playerModel.Team,
                RatingAdjustment = playerModel.RatingAdjustment

            };
        }
        public static Player ToPlayerFromCreateDTO(this CreatePlayerRequestDto playerDto)
        {
            string id = Guid.NewGuid().ToString();
            return new Player
            {
                Id = id,
                Nickname = playerDto.Nickname,
                Wins = 0,
                Losses = 0,
                Elo = 0,
                HoursPlayed = 0,
                Team = null,
                RatingAdjustment = null,
            };
        }
    }
}
