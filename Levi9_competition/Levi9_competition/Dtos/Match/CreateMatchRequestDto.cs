using System.ComponentModel.DataAnnotations;

namespace Levi9_competition.Dtos.Match
{
    public class CreateMatchRequestDto
    {
        [Required(ErrorMessage = "Team1Id is required.")]
        public string Team1Id { get; set; } = string.Empty;
        [Required(ErrorMessage = "Team2Id is required.")]
        public string Team2Id { get; set; } = string.Empty;
        public string? WinningTeamId { get; set; }
        [Required(ErrorMessage = "Duration is required.")]
        [Range(1, int.MaxValue, ErrorMessage = "Duration must be at least 1 hour.")]
        public int Duration { get; set; }
    }
}
