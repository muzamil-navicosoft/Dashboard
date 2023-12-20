using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dashboard.Utillities.Helper.Email
{
    public interface IEmailService
    {
        void SendEmail(string email, string content, string title);
    }
}
