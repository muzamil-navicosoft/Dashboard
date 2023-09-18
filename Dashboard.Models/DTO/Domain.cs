using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dashboard.Models.DTO
{  
    public class Domain
    {
        public string name { get; set; } = string.Empty;
        public string hosting_type { get; set; } = "virtual";
        public HostingSettings hosting_settings { get; set; } = new HostingSettings { ftp_login = "ftplogin", ftp_password = "testApi@1234" };
        public List<string> ipv4 { get; set; } = new List<string> { "104.219.233.15" };
        public Plan plan { get; set; } = new Plan { name = "Unlimited" };
    }
    public class HostingSettings
    {
        public string ftp_login { get; set; } = string.Empty;
        public string ftp_password { get; set; } = string.Empty;
    }

    public class Plan
    {
        public string name { get; set; } = string.Empty;
    }

}
