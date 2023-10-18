using Dashboard.DataAccess.Repo.IRepository;
using Dashboard.Models.DTO;
using Dashboard.Models.Models;
using Dashboard.Utillities.Helper;
using Mapster;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Data;

namespace Dashboard.Controllers
{
    
    public class OathController : Controller
    {
        private readonly IOathRepo oathRepo;
        private readonly IUserService userService;

        public OathController(IOathRepo oathRepo, IUserService userService)
        {
            this.oathRepo = oathRepo;
            this.userService = userService;
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
                    var test = userService.GetUserId();
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
        [HttpGet]
        [Route("CreatRole")]
        public IActionResult CreateRole()
        {
            return View();
        }
        [HttpPost]
        [Route("CreatRole")]
        public async Task<IActionResult> CreateRole(RoleDto obj)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    //obj.NormalizedName = obj.Name.ToUpper();
                    
                    var result2 = await oathRepo.CreateRoleAsync(obj);
                    if(result2.Succeeded)
                    {
                        return RedirectToAction("Index", "Home");
                    }
                    else
                    {
                        foreach (var item in result2.Errors)
                        {
                            ModelState.AddModelError("", item.Description);
                        }
                        return View();
                    }
                  
                }
            }
            catch (Exception)
            {

                throw;
            }
            return View();
        }

        [HttpGet]
        [Route("RolesList")]
        public IActionResult RolesList()
        {
            var result = oathRepo.GetRoles();
            var resul2 = result.Adapt<IEnumerable<RoleDto>>();
            return View(resul2);
        }
        public async Task<IActionResult> DeleteRole(string Id)
        {
            var role = await oathRepo.DeleteRole(Id);
            if (role.Succeeded)
            {
                return RedirectToAction("RolesList");
            }
            else
            {
                foreach (var item in role.Errors)
                {
                    ModelState.AddModelError("", item.Description);
                }
                return View();
                
            }
           

        }
        [HttpGet]
        [Route("UserList")]
        public  IActionResult UsersList()
        {
            var reuslt = oathRepo.GetUsers();
            var result2 = reuslt.Adapt<IEnumerable<SignUpDto>>();
            return View(result2);
        }
        public async Task<IActionResult> UserRoles(string id)
        {
            var result = await oathRepo.GetUserRoles(id);
           // var result2 = result.Adapt<IEnumerable<RoleDto>>();
            return View(result);
        }
        [HttpGet]
        [Route("AddtoRole")]
        public async Task<IActionResult> AddtoRole(string Id)
        {
            //var result = oathRepo.GetRoles();
            //var addToRoleDto = new AddToRoleDto();
            //foreach (var item in result)
            //{
            //    addToRoleDto.Roles.Add(item.Name);
            //}

            var allRoles =  oathRepo.GetRoles();
            var userRoles = await oathRepo.GetUserRoles(Id);


            var addToRoleDto = new AddToRoleDto
            {
                Id = Id,
                Roles = allRoles.Select(role => role.Name).ToList(),
                SelectedRoles = userRoles.ToList(),
                PrviouslySelectedRoles = userRoles.ToList()
            };
            //TempData["PrviouslySelectedRoles"] = addToRoleDto.PrviouslySelectedRoles;

            return View(addToRoleDto);
        }


        [HttpPost]
        [Route("AddtoRole")]
        public async Task<IActionResult> AddtoRole(AddToRoleDto obj)
        {
            
            //obj.PrviouslySelectedRoles = TempData["PrviouslySelectedRoles"] as List<string>;
            if (ModelState.IsValid)
            {
                var remove = await oathRepo.RemoveUserRole(obj.Id, obj.PrviouslySelectedRoles);
                var result = await oathRepo.AddUserInRole(obj.Id, obj.SelectedRoles);
                if (!result.Succeeded)
                {
                    foreach (var item in result.Errors)
                    {
                        ModelState.AddModelError("", item.Description);
                    }
                    return View();
                }
                ModelState.Clear();
                return RedirectToAction("UsersList");

            }


            return View();
        }
    }
}
