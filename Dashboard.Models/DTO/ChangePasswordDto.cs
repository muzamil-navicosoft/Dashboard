using System.ComponentModel.DataAnnotations;


namespace Dashboard.Models.DTO;

public class ChangePasswordDto
{
    [DataType(DataType.Password)]
    [Display(Name = "Old Password")]
    [Required]
    public string CurrentPassword { get; set; } = string.Empty;
    [DataType(DataType.Password)]
    [Display(Name = "New Password")]
    [Compare("ConfirmPassword", ErrorMessage ="New Passowrd Dose Not Match")]
    [Required]
    public string NewPassword { get; set;} = string.Empty;
    [DataType(DataType.Password)]
    [Display(Name = "Confirm Password")]
    [Required]
    public string ConfirmPassword { get; set;} = string.Empty;
}
