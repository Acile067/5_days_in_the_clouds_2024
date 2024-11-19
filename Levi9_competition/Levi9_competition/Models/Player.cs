namespace Levi9_competition.Models
{
    public class Player
    {
        public string Id { get; set; } = string.Empty;
        public string Nickname { get; set; } = string.Empty;
        public int Wins { get; set; }
        public int Losses { get; set; }
        public int Elo { get; set; }
        public int HoursPlayed { get; set; }
        public string? Team { get; set; }
        public int? RatingAdjustment { get; set; }

    }
}
