using Dashboard.DataAccess.Repo.IRepository;
using Dashboard.DataAccess.UnitOfWork;
using Dashboard.Models.DTO;
using Dashboard.Models.Models;
using Dashboard.Utillities.Helper;
using Hangfire.Annotations;
using Mapster;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Data;

namespace Dashboard.Controllers
{

    public class OathController : Controller
    {
        private readonly IOathRepo oathRepo;
        private readonly IUserService userService;
        private readonly IUnitOfWork unitOfWork;

        public OathController(IOathRepo oathRepo, IUserService userService, IUnitOfWork unitOfWork)
        {
            this.oathRepo = oathRepo;
            this.userService = userService;
            this.unitOfWork = unitOfWork;
        }

        [Authorize]
        [Route("/")]
        public async Task<IActionResult> Index()
        {
            var billCount = unitOfWork.billing.CustomeGetAll().Include(x => x.ClientForm).Where(x => !x.IsPaid && x.ClientForm.isActive).AsNoTracking().Count();
            var ticketCount = unitOfWork.ticket.CustomeGetAll().Include(x => x.ClientForm).Where(x => x.IsActive).AsNoTracking().Count();
            var ActiveuserCount = await unitOfWork.User
                                   .CustomeGetAll()
                                   .Where(x => x.isActive && x.isAproved && !x.isDeleted)
                                   .AsNoTracking()
                                   .ToListAsync();
            var PendinguserCount = await unitOfWork.User
                                   .CustomeGetAll()
                                   .Where(x => x.isActive && !x.isAproved && !x.isDeleted)
                                   .AsNoTracking()
                                   .ToListAsync();
            var deactiveuserCount = await unitOfWork.User
                                  .CustomeGetAll()
                                  .Where(x => !x.isActive && x.isAproved && !x.isDeleted)
                                  .AsNoTracking()
                                  .ToListAsync();
            var userCount = await unitOfWork.User
                                  .CustomeGetAll()
                                  .AsNoTracking()
                                  .ToListAsync();

            var PaidbillCount = unitOfWork.billing.CustomeGetAll().Include(x => x.ClientForm).Where(x => x.IsPaid && x.ClientForm.isActive).AsNoTracking().Count();

            var count = new Counter()
            {
                ActiveBillingCounter = billCount,
                ActiveTicketCounter = ticketCount,
                ActiveUserCounter = ActiveuserCount.Count(),
                DeActiveUserCounter = deactiveuserCount.Count(),
                PendingUserCounter = PendinguserCount.Count(),
                AllUserCounter = userCount.Count(),
                PaidBillingCounter = PaidbillCount,
            };
            return View(count);
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

        //[Route("signnup")]
        //[HttpPost]
        //public async Task<IActionResult> signUp(string fname, string lname, string email, string password , string ConfirmPassword)
        //{
        //    SignUpDto obj = new SignUpDto
        //    {
        //        FirstName = fname,
        //        LastName = lname,
        //        Email = email,
        //        Password = password,
        //        ConfirmPassword = ConfirmPassword
        //    };
        //    if (ModelState.IsValid)
        //    {
        //        var result = await oathRepo.CreateUserAsync(obj);
        //        if (!result.Succeeded)
        //        {
        //            foreach (var item in result.Errors)
        //            {
        //                ModelState.AddModelError("", item.Description);
        //            }
        //            return View();
        //        }
        //        ModelState.Clear();
        //        return RedirectToAction("Index", "Home");
        //    }
        //    else
        //    {
        //        ModelState.AddModelError("", "invalid Fields");
        //        return View();
        //    }

        //}

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
                    // For Applying the logic of Client Dashboard View
                    if(User.IsInRole("Client"))
                        return RedirectToAction("UserDetail", "UserForm");
                    return RedirectToAction("Index", "oath");
                }
                else if(result.IsNotAllowed)
                {
                    ModelState.AddModelError("", "Please Confirem your Email Before Login");
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
            return RedirectToAction("login");
        }
        [Route("Change-Password")]
        [HttpGet]
        public IActionResult changePassword()
        {
            return View();
        }
        [Route("Change-Password")]
        [HttpPost]
        public async Task<IActionResult> changePassword(ChangePasswordDto obj)
        {
            if (ModelState.IsValid)
            {
                var userId = userService.GetUserId();
                var user = await oathRepo.GetUser(userId);
                var result = await oathRepo.ChangePassword(user, obj.CurrentPassword, obj.NewPassword);

                if (!result.Succeeded)
                {
                    foreach (var item in result.Errors)
                    {
                        ModelState.AddModelError("", item.Description);
                    }
                    return View();
                }
                return View();
            }
            else
            {
                ModelState.Clear();
                return RedirectToAction("Index", "Home");
            }

        }
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
                    if (result2.Succeeded)
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
        public IActionResult UsersList()
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

            var allRoles = oathRepo.GetRoles();
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

        [HttpGet]
        [Route("confirm-email")]
        public async Task<IActionResult> ConfirmEmail(string uid, string token)
        {
            
            if (!string.IsNullOrEmpty(uid) && !string.IsNullOrEmpty(token))
            {
                token = token.Replace(" ", "+");
                var result = await oathRepo.ConfirmEmail(uid, token);
                if (result.Succeeded)
                {
                    ViewBag.isSuccess = true;
                    return View();
                }
                else
                {
                    foreach (var item in result.Errors)
                    {
                        ModelState.AddModelError("", item.Description);
                    }
                    ViewBag.isSuccess = false;
                    return View();

                }
            }
            else
            {       
                return View();
            }
        } 
        [HttpPost]
        [Route("confirm-email")]
        public async Task<IActionResult> ConfirmEmail(string emeil,ResendEmailDto? obj)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var user = await oathRepo.GetUserByEmailAsync(emeil);
                    if( user != null)
                    {
                        if (user.EmailConfirmed)
                        {
                            obj.IsEmailConfirmed = true;
                            // message for  Accont already verfied
                            return View();
                        }
                        // for Genrating Email
                        await oathRepo.GenrateTokenAndSendEmailAsync(user);
                        obj.IsEmailSent = true;
                        ModelState.Clear();
                        return View();
                    }
                    // For Error when user Doesnot existe with specific Email
                    return View();
                }
                else
                {
                    ModelState.AddModelError("", "kindly Renter the Email Address");
                    return View();
                }
            }
            catch (Exception)
            {
               
                throw;
            }

        }

        [HttpGet]
        [Route("Forgot-password")]
        public IActionResult ForgotPassword()
        {
            return View();
        }
        [HttpPost]
        [Route("Forgot-password")]
        public async Task<IActionResult> ForgotPassword(ForgotPasswordDto obj)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var user = await oathRepo.GetUserByEmailAsync(obj.Email);
                    if (user != null)
                    {
                       
                        // for Genrating Email
                        await oathRepo.GenrateForgotPasswordTokenAndSendEmailAsync(user);
                        obj.IsEmailSent = true;
                        ModelState.Clear();
                        return View(obj);
                    }
                    // For Error when user Doesnot existe with specific Email
                    ModelState.AddModelError("", "Please Entery Your Correct Registerd Email Address");
                    return View();
                }
                ModelState.AddModelError("", "Please Entery Your Registerd Email Address its required");
                return View();
            }
            catch (Exception)
            {

                throw;
            }

        }

        [HttpGet]
        [Route("reset-password")]
        public IActionResult resetpassword(string uid, string token)
        {
            ResetPasswordDto obj = new ResetPasswordDto
            {
                UserId = uid,
                Token = token
            };
            return View(obj);
        }
        [HttpPost]
        [Route("reset-password")]
        public async Task<IActionResult> resetpassword(ResetPasswordDto obj)
        {

            if (!string.IsNullOrEmpty(obj.UserId) && !string.IsNullOrEmpty(obj.Token))
            {
                obj.Token = obj.Token.Replace(" ", "+");
                var result = await oathRepo.ConfirmPasswordasync(obj.UserId, obj.Token,obj.NewPassword);
                if (result.Succeeded)
                {
                    ViewBag.isSuccess = true;
                    obj.IsPasswordReset = true;
                    return View();
                }
                else
                {
                    foreach (var item in result.Errors)
                    {
                        ModelState.AddModelError("", item.Description);
                    }
                    ViewBag.isSuccess = false;
                    return View();

                }
            }
            else
            {
                return View();
            }
        }
    }
}
