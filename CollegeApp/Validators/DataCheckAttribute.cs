using System.ComponentModel.DataAnnotations;

namespace CollegeApp.Validators
{
    public class DataCheckAttribute : ValidationAttribute
    {
        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            var date = (DateTime?)value;

            if (date < DateTime.Now)
            {
                return new ValidationResult("The Admission date can NOT be in the past");
            }

            return ValidationResult.Success;
        }
    }
}
