using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dashboard.Models.DTO
{
    public class ResendEmailDto
    {
        public string Email { get; set; } = string.Empty;
        public bool IsEmailConfirmed { get; set; }
        public bool IsEmailSent { get; set; }
    }
}
