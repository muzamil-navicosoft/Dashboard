using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dashboard.Models.DTO
{
    public class Counter
    {
        public int ActiveTicketCounter { get; set; }
        public int ActiveBillingCounter { get; set; }
        public int ActiveUserCounter { get; set; }
        public int DeActiveUserCounter { get; set; }
        public int PaidBillingCounter { get; set; }
    }
}
