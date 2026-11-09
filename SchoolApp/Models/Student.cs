using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using SchoolApp.Validation;

namespace SchoolApp.Models
{
    public class Student
    {
        public int StudentId { get; set; }

        // STUDENT NUMBER
        [Display(Name = "Student No.")]
        [Required(ErrorMessage = "Please enter the student number.")]
        [RegularExpression(@"^\d{4}-\d{4}$", ErrorMessage = "Use the format 2026-0001.")]
        public string? StudentNo { get; set; }

        // NAME
        [Display(Name = "Full Name")]
        [Required(ErrorMessage = "Please enter the student's name. - Yiohan")]
        [StringLength(100, ErrorMessage = "The name can't be longer than 100 characters.")]
        public string? Name { get; set; }

        // EMAIL
        [Display(Name = "Email Address")]
        [Required(ErrorMessage = "Please enter an email address.")]
        [EmailAddress(ErrorMessage = "Please enter a valid email, like ana@example.com.")]
        public string? Email { get; set; }

        // CONFIRM EMAIL
        [Display(Name = "Confirm Email")]
        [Required(ErrorMessage = "Please type the email again.")]
        [NotMapped]
        [Compare("Email", ErrorMessage = "Emails do not match.")]
        public string? ConfirmEmail { get; set; }

        // PHONE
        [Display(Name = "Phone Number")]
        [Phone(ErrorMessage = "Please enter a valid phone number.")]
        public string? Phone { get; set; }

        // AGE
        [Range(16, 60, ErrorMessage = "Age must be from 16 to 60.")]
        public int? Age { get; set; }




        [Display(Name = "Birth Date")]
        // Shows a date picker without the time
        [DataType(DataType.Date)]
        // Our custom rule from Validation/NotFutureDateAttribute.cs
        [NotFutureDate(ErrorMessage = "Birth date can't be in the future.")]
        public DateTime? BirthDate { get; set; }
    }
}
