using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dashboard.Models.DTO
{
    public class SignUpDto
    {
        public string Id { get; set; } = string.Empty;
        [Required(ErrorMessage = "Kinldy Enter Your FirstName")]
        [MaxLength(50)]
        [Display(Name = "First Name")]
        public string FirstName { get; set; } = string.Empty;

        [Required(ErrorMessage = "Kinldy Enter Your LastName")]
        [MaxLength(50)]
        [Display(Name = "Last Name")]
        public string LastName { get; set; } = string.Empty;

        [Required(ErrorMessage = "Email Field is Required")]
        [Display(Name = "Email Address")]
        [DataType(DataType.EmailAddress)]
        [RegularExpression(@"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,3})+)$", ErrorMessage = "This Email is not valid.")]
        public string Email { get; set; } = string.Empty;

        [Required(ErrorMessage = "Password is Required")]
        [Display(Name = "Password")]
        [Compare("ConfirmPassword", ErrorMessage = "Password Does Not Match")]
        [DataType(DataType.Password)]
        public string Password { get; set; } = string.Empty;

        [Required(ErrorMessage = "Please Conform your Password")]
        [Display(Name = "Conform Password")]
        [DataType(DataType.Password)]
        public string ConfirmPassword { get; set; } = string.Empty;
    }
}
