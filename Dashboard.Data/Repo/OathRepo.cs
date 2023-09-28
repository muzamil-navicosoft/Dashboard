using Dashboard.DataAccess.Repo.IRepository;
using Dashboard.Models.DTO;
using Dashboard.Models.Models;
using Microsoft.AspNetCore.Identity;


namespace Dashboard.DataAccess.Repo
{

    public class OathRepo : IOathRepo
    {
        private readonly UserManager<CustomeUser> userManager;
        private readonly SignInManager<CustomeUser> signInManager;

        //private readonly IGenralPurpose genralPurpose;

        public OathRepo(UserManager<CustomeUser> userManager, SignInManager<CustomeUser> signInManager)
        {
            this.userManager = userManager;
            this.signInManager = signInManager;
        }

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
            return result;
        }

        public async Task<SignInResult> LoginAsync(SignInDto obj)
        {
            var result = await signInManager.PasswordSignInAsync(obj.Email, obj.Password, obj.RemaberMe, false);
            return result;
        }

        public async Task logout()
        {
            await signInManager.SignOutAsync();
        }

        

    }
}