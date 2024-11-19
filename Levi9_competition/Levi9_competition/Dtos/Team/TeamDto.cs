using Levi9_competition.Dtos.Player;

namespace Levi9_competition.Dtos.Team
{
    public class TeamDto
    {
        public string Id { get; set; } = string.Empty;
        public string TeamName { get; set; } = string.Empty;
        public List<PlayerDto> Players { get; set; } = new List<PlayerDto>();
    }
}
