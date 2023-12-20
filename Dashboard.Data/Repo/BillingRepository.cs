using Dashboard.Data;
using Dashboard.DataAccess.Repo.IRepository;
using Dashboard.Models.Models;


namespace Dashboard.DataAccess.Repo
{
    public class BillingRepository : Repository<BillingInfo>, IBillingRepository
    {
        private readonly ProjectContext _projectContext;
        public BillingRepository(ProjectContext projectContext) : base(projectContext)
        {
            _projectContext = projectContext;
        }
        public void update(BillingInfo obj)
        {
            _projectContext.Update(obj);
        }
    }
}
