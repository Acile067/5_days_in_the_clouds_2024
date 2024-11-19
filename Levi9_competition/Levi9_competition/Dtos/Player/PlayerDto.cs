namespace Levi9_competition.Dtos.Player
{
    public class PlayerDto
    {
        public string Id { get; set; } = string.Empty;
        public string Nickname { get; set; } = string.Empty;
        public int Wins { get; set; } = 0;
        public int Losses { get; set; } = 0;
        public int Elo { get; set; } = 0;
        public int HoursPlayed { get; set; } = 0;
        public string? Team { get; set; } = string.Empty;
        public string? RatingAdjustment { get; set; } = string.Empty;
    }
}
