using Dashboard.Data;
using Dashboard.DataAccess.Repo.IRepository;
using Dashboard.Models.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Dashboard.DataAccess.Repo
{
    public class UserRepository : Repository<ClientForm>, IUserRepository
    {
        private readonly ProjectContext _projectContext;
        public UserRepository(ProjectContext projectContext) : base(projectContext) 
        {
                _projectContext = projectContext;   
        }
        public void update(ClientForm obj)
        {
            _projectContext.Update(obj);
        }
    }
}
