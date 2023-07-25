﻿using System.ComponentModel.DataAnnotations;

namespace Dashboard.Models
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
        public DateTime? DiscountneDate { get; set; }
        public bool isAproved { get; set; } = false;
        public bool isActive { get; set; } = true;

    }
}