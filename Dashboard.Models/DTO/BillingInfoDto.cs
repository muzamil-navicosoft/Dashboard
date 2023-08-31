using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dashboard.Models.DTO
{
    public class BillingInfoDto
    {
        public int Id { get; set; }
        public double Amount { get; set; }
        public bool IsPaid { get; set; } = false;

        public int? ClientFormId { get; set; }
        public string? Email { get; set; }
    }
}
