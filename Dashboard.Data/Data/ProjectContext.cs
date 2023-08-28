using Dashboard.Models.Models;
using Microsoft.EntityFrameworkCore;

namespace Dashboard.Data
{
    public class ProjectContext : DbContext
    {
        public ProjectContext(DbContextOptions<ProjectContext> options) : base(options)
        {
        }

        public DbSet<ClientForm> ClientForm { get; set; }
        public DbSet<Ticket> Ticket { get; set; }


    }
}
