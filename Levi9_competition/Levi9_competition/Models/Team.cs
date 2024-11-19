namespace Levi9_competition.Models
{
    public class Team
    {
        public string Id { get; set; } = string.Empty;
        public string TeamName { get; set; } = string.Empty;
        public List<Player> Players { get; set; } = new List<Player>();
    }
}
