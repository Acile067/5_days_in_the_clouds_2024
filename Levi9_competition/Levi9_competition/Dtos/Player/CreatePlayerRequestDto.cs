using System.ComponentModel.DataAnnotations;

namespace Levi9_competition.Dtos.Player
{
    public class CreatePlayerRequestDto
    {
        [Required(ErrorMessage = "Nickname is required.")]
        [MinLength(1, ErrorMessage = "Symbol must be 1 characters")]
        [MaxLength(20, ErrorMessage = "Symbol cannot be over 20 characters")]
        public string Nickname { get; set; } = string.Empty;
    }
}
