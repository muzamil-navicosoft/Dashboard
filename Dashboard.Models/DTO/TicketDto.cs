using Dashboard.Models.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dashboard.Models.DTO
{
    public class TicketDto
    {
        public int Id { get; set; }
        [Required]
        [MaxLength(100)]
        public string Title { get; set; } = string.Empty;
        [Required]
        [MinLength(50), MaxLength(500)]
        public string Description { get; set; } = string.Empty;
        public string? Resolution { get; set; } = string.Empty;
        public string? Department { get; set; } = string.Empty;
        public bool IsActive { get; set; } = true;
        public int? ClientFormId { get; set; }
        public string? Email { get; set; }
        public DateTime DateCreated { get; set; } = DateTime.Now;
        public DateTime? DateReolved { get; set; }
    }
}
