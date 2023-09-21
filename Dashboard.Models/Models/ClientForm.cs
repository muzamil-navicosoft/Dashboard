using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Dashboard.Models.Models
{
    public class ClientForm
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Logo { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string? BlackBaudApiId { get; set; } 
        public string? Password { get; set; } 
        public string? SubDomain { get; set; } 
        public DateTime RequestDate { get; set; }
        public DateTime? AproveDate { get; set; }
        public DateTime? DiscontinueDate { get; set; }
        public bool isAproved { get; set; } 
        public bool isActive { get; set; } 
        public bool isDeleted { get; set; }
        public bool? isBilledMonthly { get; set; } 
        public ICollection<Ticket>? Tickets { get; set; }

        public ICollection<BillingInfo>? BillingInfos { get; set; }
        public double? OneTimeBill { get; set; }
    }
}
