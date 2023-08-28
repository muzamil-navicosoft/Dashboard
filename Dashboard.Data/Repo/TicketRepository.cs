using Dashboard.Data;
using Dashboard.DataAccess.Repo.IRepository;
using Dashboard.Models.Models;


namespace Dashboard.DataAccess.Repo
{
    public class TicketRepository : Repository<Ticket>, ITicketRepository
    {
        private readonly ProjectContext _projectContext;
        public TicketRepository(ProjectContext projectContext) : base(projectContext)
        {
            _projectContext = projectContext;
        }
        public void update(Ticket obj)
        {
            _projectContext.Update(obj);
        }
    }
}
