using Levi9_competition.Helpers;
using System.ComponentModel.DataAnnotations;

namespace Levi9_competition.Dtos.Team
{
    public class CreateTeamRequestDto
    {
        [Required(ErrorMessage = "TeamName is required.")]
        [MinLength(1, ErrorMessage = "Symbol must be 1 characters")]
        [MaxLength(20, ErrorMessage = "Symbol cannot be over 20 characters")]
        public string TeamName { get; set; } = string.Empty;

        [Required(ErrorMessage = "Players list is required.")]
        [MinLength(5, ErrorMessage = "Team must have exactly 5 players.")]
        [MaxLength(5, ErrorMessage = "Team must have exactly 5 players.")]
        [UniqueItems(ErrorMessage = "Players list must contain unique GUIDs.")]
        public List<string> Players { get; set; } = new();
    }
}
