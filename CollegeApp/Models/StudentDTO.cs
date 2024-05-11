using CollegeApp.Validators;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations;

namespace CollegeApp.Models
{
    public class StudentDTO
    {
        [ValidateNever]
        public int Id { get; set; }

        [Required(ErrorMessage = "Name is required")]
        [StringLength(100)]
        public string Name { get; set; }

        [EmailAddress(ErrorMessage = "Incorrect email format")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Address is required")]
        [StringLength(100)]
        public string Address { get; set; }

        //[DataCheck]
        //public DateTime AdmissionDate { get; set; }

        //[Range(18, 150)]
        //public int Age { get; set; }

        //public string Password { get; set; }

        //[Compare(nameof(Password))]
        //public string confirmPassword { get; set; }
    }
}
