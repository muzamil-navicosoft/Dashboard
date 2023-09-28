using Dashboard.Data;
using Dashboard.Models.Models;
using Dashboard.Utillities.Helper;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Build.Evaluation;
using System.Diagnostics;

namespace Dashboard.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IUserService userservice;
        private readonly RoleManager<IdentityRole> roleManager;
        private readonly UserManager<CustomeUser> userManager;
        private readonly ProjectContext projectContext;

        public HomeController(ILogger<HomeController> logger, IUserService userservice,
                RoleManager<IdentityRole> roleManager, UserManager<CustomeUser> userManager,
                ProjectContext projectContext)
        {
            _logger = logger;
            this.userservice = userservice;
            this.roleManager = roleManager;
            this.userManager = userManager;
            this.projectContext = projectContext;
        }

        public async Task<IActionResult> Index()
        {
            var result = userservice.GetUserId();
            var result2 = User.FindFirst("userId")?.Value;
            var authChek = userservice.isAuthenticated();
            if(result2 != null)
            {
                var user = await userManager.FindByIdAsync(result2);
                var userRoles = await userManager.GetRolesAsync(user);
                var userRoleName = userRoles[0];
            }
              
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}