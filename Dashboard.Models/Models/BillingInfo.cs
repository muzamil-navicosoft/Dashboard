﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dashboard.Models.Models
{
    public class BillingInfo
    {
        public int Id { get; set; }
        public double Amount { get; set; }
        public bool IsPaid { get; set; }
        public string Month { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public DateTime DueDate { get; set; }
        
        public DateTime? PaidDate { get; set; }
        public int? ClientFormId { get; set; }
        public ClientForm? ClientForm { get; set; }
    }
}
