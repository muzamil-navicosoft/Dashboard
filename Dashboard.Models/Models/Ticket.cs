﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dashboard.Models.Models
{
    public class Ticket
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string Resolution { get; set; } = string.Empty;
        public bool IsActive { get; set; } = true;
        public int? ClientFormId { get; set; }
        public ClientForm? ClientForm { get; set; }
        public DateTime DateCreated { get; set; } 
        public DateTime? DateReolved { get; set; }
    }
}