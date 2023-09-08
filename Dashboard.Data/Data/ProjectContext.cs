using Dashboard.Models.DTO;
using Dashboard.Models.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Dashboard.Data
{
    public class ProjectContext : IdentityDbContext
    {
        public ProjectContext(DbContextOptions<ProjectContext> options) : base(options)
        {
        }

        public DbSet<ClientForm> ClientForm { get; set; }
        public DbSet<Ticket> Ticket { get; set; }


    }
}
