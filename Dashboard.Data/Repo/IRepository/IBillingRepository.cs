using Dashboard.Models.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dashboard.DataAccess.Repo.IRepository
{
    public interface IBillingRepository : IRepository<BillingInfo>
    {
        void update (BillingInfo obj);
    }
}
