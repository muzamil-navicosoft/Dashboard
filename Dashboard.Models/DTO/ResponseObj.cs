using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dashboard.Models.DTO
{
    public class ResponseObj
    {
        public int code { get; set; }
        public string stdout { get; set; } = string.Empty;
        public string stderr { get; set; } = string.Empty;
    }
}
