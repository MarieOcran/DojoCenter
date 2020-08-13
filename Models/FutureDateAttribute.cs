using System;
using System.ComponentModel.DataAnnotations;

namespace FinalExam.Models
{
    public class FutureDateAttribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
    {
        if ((DateTime)value < DateTime.Now)
            return new ValidationResult("Date/Time Must Be In The Future");
        return ValidationResult.Success;
    }
    }
}