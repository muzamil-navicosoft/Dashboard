using Dashboard.Models.DTO;
using Dashboard.Models.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Data;
using System.Reflection.Emit;

namespace Dashboard.Data
{
    public class ProjectContext : IdentityDbContext<CustomeUser>
    {
        public ProjectContext(DbContextOptions<ProjectContext> options) : base(options)
        {
        }

        public DbSet<ClientForm> ClientForm { get; set; }
        public DbSet<Ticket> Ticket { get; set; }

        protected override void OnModelCreating(ModelBuilder model)
        {
            base.OnModelCreating(model);
        
        // Seeding a  'Administrator, Billing ,  Client , Tickitting' role to AspNetRoles table

            model.Entity<IdentityRole>().HasData(
                new IdentityRole 
                {   Id = "800e2e71-cae7-4c8c-ab5d-80e9be55ad6d", 
                    Name = "Admin", 
                    NormalizedName = "ADMIN" },
                new IdentityRole
                {
                    Id = "901dc4e1-3a0f-43bc-a7ed-f4885512281a",
                    Name = "Billing",
                    NormalizedName = "BILLING"
                },
                new IdentityRole
                {
                    Id = "6415d800-fa65-4fa0-b547-d53aeba70950",
                    Name = "Client",
                    NormalizedName = "CLIENT"
                },
                new IdentityRole
                {
                    Id = "8eaad183-6838-46d0-b008-cc914ddedbda",
                    Name = "Tickitting",
                    NormalizedName = "TICKITTING"
                });

            //a hasher to hash the password before seeding the user to the db
            var hasher = new PasswordHasher<CustomeUser>();

            model.Entity<CustomeUser>().HasData(
                new CustomeUser
            {
                //Id = new Guid().ToString(),
                //UserName = "admin@gmail.com",
                //NormalizedUserName = "ADMIN@GMAIL.COM",
                //Email = "admin@gmail.com",
                //NormalizedEmail = "ADMIN@GMAIL.COM",
                //EmailConfirmed = true,
                //LockoutEnabled = false,
                //SecurityStamp = Guid.NewGuid().ToString()

                Id = "f06625f3-5cf0-487e-be5c-c76242561bf8",
                FirstName = "Navicosoft",
                LastName = "Admin",
                UserName = "admin@gmail.com",
                NormalizedUserName = "ADMIN@GMAIL.COM",
                Email = "admin@gmail.com",
                NormalizedEmail = "ADMIN@GMAIL.COM",
                SecurityStamp = Guid.NewGuid().ToString("D"),
                PasswordHash = hasher.HashPassword(null,"Admin@1234"),
                LockoutEnabled = true,
            });


            model.Entity<IdentityUserRole<string>>().HasData(
            new IdentityUserRole<string>
            {
                RoleId = "800e2e71-cae7-4c8c-ab5d-80e9be55ad6d",
                UserId = "f06625f3-5cf0-487e-be5c-c76242561bf8"
            }
        );
        }


        }
    }
