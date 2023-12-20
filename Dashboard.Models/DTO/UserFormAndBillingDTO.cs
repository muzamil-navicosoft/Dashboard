using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dashboard.Models.DTO
{
    public class UserFormAndBillingDTO
    {
        public BillingInfoDto? BillingInfo { get; set; }
        public ClientFormDto? ClientDto { get; set; }
    }
}
