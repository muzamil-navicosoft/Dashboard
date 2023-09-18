using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dashboard.Models.DTO
{
    public class SubDomain
    {
        public List<string> @params { get; set; } = new List<string> { "--create", "-domain", "navedge.co", "-www-root" };
    }
}
