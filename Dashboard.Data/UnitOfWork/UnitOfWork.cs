using Dashboard.Data;
using Dashboard.DataAccess.Repo;
using Dashboard.DataAccess.Repo.IRepository;
using Dashboard.Models.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dashboard.DataAccess.UnitOfWork
{
    public class UnitOfWork : IUnitOfWork
    {
        public IUserRepository User { get; private set; }
        public ITicketRepository ticket { get; private set; }
        private readonly ProjectContext _projectContext;
        public UnitOfWork(ProjectContext projectContext)
        {
            _projectContext = projectContext;
            User = new UserRepository(projectContext);
            ticket = new TicketRepository(projectContext);
        }
        public void Save()
        {
            _projectContext.SaveChanges();
        }
    }
}
