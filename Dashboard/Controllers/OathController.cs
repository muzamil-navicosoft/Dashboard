using Dashboard.DataAccess.Repo.IRepository;
using Dashboard.Models.DTO;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Dashboard.Controllers
{
    
    public class OathController : Controller
    {
        private readonly IOathRepo oathRepo;

        public OathController(IOathRepo oathRepo)
        {
            this.oathRepo = oathRepo;
        }

        [Route("signup")]
        public IActionResult signUp()
        {
            return View();
        }
        [Route("signup")]
        [HttpPost]
        public async Task<IActionResult> signUp(SignUpDto obj)
        {
            if (ModelState.IsValid)
            {
                var result = await oathRepo.CreateUserAsync(obj);
                if (!result.Succeeded)
                {
                    foreach (var item in result.Errors)
                    {
                        ModelState.AddModelError("", item.Description);
                    }
                    return View();
                }
                ModelState.Clear();
                return RedirectToAction("Index", "Home");
            }
            else
            {
                ModelState.AddModelError("", "invalid Fields");
                return View();
            }

        }

        [Route("login")]
        public IActionResult Login()
        {
            return View();
        }
        [Route("login")]
        [HttpPost]
        public async Task<IActionResult> Login(SignInDto obj, string ReturnUrl = "")
        {
            if (ModelState.IsValid)
            {
                var result = await oathRepo.LoginAsync(obj);
                if (result.Succeeded)
                {
                    if (!string.IsNullOrEmpty(ReturnUrl))
                    {
                        return LocalRedirect(ReturnUrl);
                    }
                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    ModelState.AddModelError("", "Invalid Crdentials");

                }

            }
            return View();
        }

        public async Task<IActionResult> logout()
        {
            await oathRepo.logout();
            return RedirectToAction("index", "Home");
        }
        [Route("Change-Password")]
        [HttpGet]
        public IActionResult changePassword()
        {
            return View();
        }
        //[HttpPost]
        //public async Task<IActionResult> changePassword(ChangePasswordDto obj)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        //var result = await oathRepo.ChnagePasswordAsync(obj);
        //        //if (result.Succeeded)
        //        //{
        //        //    return RedirectToAction("Index", "Home");
        //        //}
        //        //else
        //        //{
        //        //    foreach (var item in result.Errors)
        //        //    {
        //        //        ModelState.AddModelError("", item.Description);
        //        //    }
        //        //}
        //    }

        //    return View(obj);
        //}
    }
}
