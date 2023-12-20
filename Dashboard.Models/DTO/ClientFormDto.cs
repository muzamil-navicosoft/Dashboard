using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Dashboard.Models.DTO
{
    public class ClientFormDto
    {
        public int Id { get; set; }

        [Required(ErrorMessage ="Please Enter Your Company Name")]
        [Display(Name = "Company Name")]
        public string Name { get; set; } = string.Empty;
        [Display(Name = "Company Logo")]
        public string? Logo { get; set; } = string.Empty;
        [Display(Name = "Company Logo")]
        public IFormFile? LogoImage { get; set; }

        [Required(ErrorMessage="Please Enter your Email Address")]
        [EmailAddress]
        [RegularExpression(@"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,3})+)$", ErrorMessage = "This Email is not valid.")]
        public string Email { get; set; } = string.Empty;
        public string? BlackBaudApiId { get; set; } 
        public string? Password { get; set; }
        [Display(Name = "Sub Domain")]
        public string? SubDomain { get; set; }

        [DataType(DataType.Date)]
        public DateTime RequestDate { get; set; } = DateTime.Now;
        [DataType(DataType.Date)]
        public DateTime? AproveDate { get; set; }
        [DataType(DataType.Date)]
        public DateTime? DiscontinueDate { get; set; }
        public bool isAproved { get; set; } = false;
        public bool isActive { get; set; } = true;
        public bool isDeleted { get; set; } = false;
        public bool isBilledMonthly { get; set; } = false;
        public List<TicketDto>? Tickets { get; set; }

        public List<BillingInfoDto>? BillingInfos { get; set; }
        public double? OneTimeBill { get; set; }

    }
}
