namespace Levi9_competition.Models
{
    public class Match
    {
        public string Id { get; set; } = string.Empty;
        public string Team1Id { get; set; } = string.Empty;
        public string Team2Id { get; set; } = string.Empty;
        public string? WinningTeamId { get; set; }
        public int Duration { get; set; }
    }
}
