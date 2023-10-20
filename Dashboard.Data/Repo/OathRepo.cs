using Dashboard.DataAccess.Repo.IRepository;
using Dashboard.Models.DTO;
using Dashboard.Models.Models;
using Microsoft.AspNetCore.Identity;
using Dashboard.Utillities.Helper.Email;

using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace Dashboard.DataAccess.Repo
{

    public class OathRepo : IOathRepo
    {
        private readonly UserManager<CustomeUser> userManager;
        private readonly SignInManager<CustomeUser> signInManager;
        private readonly RoleManager<IdentityRole> roleManager;
        private readonly IEmailService emailService;

        //private readonly IGenralPurpose genralPurpose;

        public OathRepo(UserManager<CustomeUser> userManager, SignInManager<CustomeUser> signInManager,
                RoleManager<IdentityRole> roleManager, IEmailService emailService)
        {
            this.userManager = userManager;
            this.signInManager = signInManager;
            this.roleManager = roleManager;
            this.emailService = emailService;
        }

        #region user
        public async Task<IdentityResult> CreateUserAsync(SignUpDto obj)
        {
            var user = new CustomeUser()
            {
                FirstName = obj.FirstName,
                LastName = obj.LastName,
                Email = obj.Email,
                UserName = obj.Email
            };
            var result = await userManager.CreateAsync(user, obj.Password);
            if(result.Succeeded) 
            {
                 var token = await userManager.GenerateEmailConfirmationTokenAsync(user);
                var id = user.Id;
                 emailService.SendEmail(obj.Email, "Welcocme to Dashboard  Click on " +
                     "<a href=\"https://localhost:7124/confirm-email?uid="+id+"&token="+token+"\""+">link </a> to Verify", "Conformation");


            }
            return result;
        }

        public async Task<SignInResult> LoginAsync(SignInDto obj)
        {
            var result = await signInManager.PasswordSignInAsync(obj.Email, obj.Password, obj.RemaberMe, false);
            return result;
        }
        public async Task<CustomeUser?> GetUser(string Id)
        {
            return await userManager.FindByIdAsync(Id);
            
        }

        public async Task logout()
        {
            await signInManager.SignOutAsync();
        }

        public async Task<IdentityResult> ChangePassword(CustomeUser user, string oldPass, string  newpass)
        {
          return  await  userManager.ChangePasswordAsync(user, oldPass, newpass);
        }
     
        #endregion

        #region Role
        public async Task<IdentityResult> CreateRoleAsync(RoleDto obj)
        {
            //var result = new IdentityRole
            //{
              
            //    Name = obj.Name,
               
            //};
             return await roleManager.CreateAsync(new IdentityRole(obj.Name));
            
            
     
        }
        public IList<IdentityRole> GetRoles()
        {
            return roleManager.Roles.ToList();
        }

        public async Task<IdentityResult> DeleteRole(string roleId)
        {
            var role = await roleManager.FindByIdAsync(roleId);
            var result = await roleManager.DeleteAsync(role);
            return result;        
        }

        #endregion

        #region User Roles
        public IList<CustomeUser> GetUsers()
        {
            var result =  userManager.Users.ToList();
            
            return result;
        }

        //Get User Roles By ID
        public async Task<IList<string>> GetUserRoles(string Id)
        {
            var result = await GetUser(Id);
            if( result == null)
            {
                return new List<string>();
            }
            else
            {
                var result2 = await userManager.GetRolesAsync(result);
                return result2;
            }
        }

        //Get User Roles By  User
        public async Task<IList<String>> GetUserRolesByUser(CustomeUser user)
        {
            var result2 = await userManager.GetRolesAsync(user);
            return result2;
        }

        public async Task<IdentityResult> AddUserInRole(string id, IList<string> role)
        {
            var result = await GetUser(id);
            if( result != null )
            {
                return await userManager.AddToRolesAsync(result, role);

            }else
            {
                return new IdentityResult();
            }
        }

        public async Task<IdentityResult> RemoveUserRole(string id, IList<string> role)
        {
            var result = await GetUser(id);
            if (result != null)
            {
                return await userManager.RemoveFromRolesAsync(result, role);

            }
            else
            {
                return new IdentityResult();
            }
        }

        public async Task<IList<CustomeUser>> GetUsersInSpecificRole(string role)
        {
            var result = await userManager.GetUsersInRoleAsync(role);
            if (result != null)
            {
                return result;
            }
            return new List<CustomeUser>();
        }
        #endregion

    }
}