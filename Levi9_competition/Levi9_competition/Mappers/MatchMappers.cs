using Levi9_competition.Dtos.Match;
using Levi9_competition.Models;

namespace Levi9_competition.Mappers
{
    public static class MatchMappers
    {
        public static Match ToMatchFromCreateDTO(this CreateMatchRequestDto matchModel)
        {
            return new Match
            {
                Team1Id = matchModel.Team1Id,
                Team2Id = matchModel.Team2Id,
                WinningTeamId = matchModel.WinningTeamId,
                Duration = matchModel.Duration
            };
        }
    }
}
