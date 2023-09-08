using Dashboard.DataAccess.Repo.IRepository;
using Dashboard.Models.DTO;
using Microsoft.AspNetCore.Identity;


namespace Dashboard.DataAccess.Repo
{

    public class OathRepo : IOathRepo
    {
        private readonly UserManager<IdentityUser> userManager;
        //private readonly SignInManager<IdentityUser> signInManager;
        //private readonly IGenralPurpose genralPurpose;

        public OathRepo(UserManager<IdentityUser> userManager)
        {
            this.userManager = userManager;
            //this.signInManager = signInManager;
        }

        public async Task<IdentityResult> CreateUserAsync(SignUpDto obj)
        {
            var user = new IdentityUser()
            {
                Email = obj.Email,
                UserName = obj.Email
            };
            var result = await userManager.CreateAsync(user, obj.Password);
            return result;
        }

        //public async Task<SignInResult> LoginAsync(SignInDto obj)
        //{
        //    var result = await signInManager.PasswordSignInAsync(obj.Email, obj.Password, obj.RemaberMe, false);
        //    return result;
        //}

        //public async Task logout()
        //{
        //    await signInManager.SignOutAsync();
        //}

        

    }
}