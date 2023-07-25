﻿using System.ComponentModel.DataAnnotations;

namespace Dashboard.DTO
{
    public class ClientFormDto
    {
        public int Id { get; set; }

        [Required(ErrorMessage ="Please Enter Your Company Name")]
        [Display(Name = "Company Name")]
        public string Name { get; set; } = string.Empty;
        public string? Logo { get; set; } = string.Empty;
        [Display(Name = "Company Logo")]
        public IFormFile? LogoImage { get; set; }

        [Required(ErrorMessage="Please Enter your Email Address")]
        [EmailAddress]
        [RegularExpression(@"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,3})+)$", ErrorMessage = "Email is not valid.")]
        public string Email { get; set; } = string.Empty;
        public string? BlackBaudApiId { get; set; } 
        public string? Password { get; set; } 

        [DataType(DataType.Date)]
        public DateTime RequestDate { get; set; } = DateTime.Now;
        [DataType(DataType.Date)]
        public DateTime? AproveDate { get; set; }
        [DataType(DataType.Date)]
        public DateTime? DiscountneDate { get; set; }
        public bool isAproved { get; set; } = false;
        public bool isActive { get; set; } = true;
    }
}