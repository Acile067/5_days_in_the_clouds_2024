using System.ComponentModel.DataAnnotations;

namespace Levi9_competition.Helpers
{
    public class UniqueItemsAttribute : ValidationAttribute
    {
        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            if (value is List<string> players && players.Distinct().Count() != players.Count)
            {
                return new ValidationResult("Players list contains duplicate entries.");
            }
            return ValidationResult.Success;
        }
    }
}
