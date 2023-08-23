using Dashboard.DataAccess.Repo.IRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dashboard.DataAccess.UnitOfWork
{
    public interface IUnitOfWork 
    {
        IUserRepository User { get; }
        void Save();
    }
}
