using Dashboard.Models.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dashboard.DataAccess.Repo.IRepository
{
    public interface IUserRepository : IRepository<ClientForm>
    {
        void update(ClientForm obj);

    }
}
