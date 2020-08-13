using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

namespace FinalExam.Models
{
   public class StrongPassword : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
    {
            var hasNumber = new Regex(@"[0-9]+");
            var hasUpperChar = new Regex(@"[A-Z]+");
            var hasMiniMaxChars = new Regex(@".{8,15}");
            var hasLowerChar = new Regex(@"[a-z]+");
            var hasSymbols = new Regex(@"[!@#$%^&*()_+=\[{\]};:<>|./?,-]");
            if (!hasSymbols.IsMatch((string)value))
                return new ValidationResult("Password must have at least one special character");
            return ValidationResult.Success;
    }
    }
}