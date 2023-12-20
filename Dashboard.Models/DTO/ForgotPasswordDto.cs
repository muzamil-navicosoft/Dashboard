using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dashboard.Models.DTO
{
    public class ForgotPasswordDto
    {
        [Required(ErrorMessage = "Please Enter your Registerd Email-Address")]
        [EmailAddress]
        [RegularExpression(@"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,3})+)$", ErrorMessage = "This Email is not valid.")]
        public string Email { get; set; } = string.Empty;
        public bool IsEmailSent { get; set; }
    }
}
