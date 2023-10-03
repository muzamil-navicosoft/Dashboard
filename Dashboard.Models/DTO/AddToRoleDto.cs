using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dashboard.Models.DTO
{
    public class AddToRoleDto
    {
        public string Id { get; set; } = string.Empty;
        public List<string> Roles { get; set;} = new List<string>();
        public List<string> SelectedRoles { get; set; } = new List<string>();

    }
}
