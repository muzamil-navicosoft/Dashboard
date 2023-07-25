using Dashboard.Data;
using Dashboard.DTO;
using Dashboard.Helper;
using Dashboard.Models;
using Mapster;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Dashboard.Controllers
{
    public class UserForm : Controller
    {
        private readonly ProjectContext db;
        private readonly IWebHostEnvironment webHost;
        private readonly ICreateImage image;

        public UserForm(ProjectContext db, IWebHostEnvironment webHost, ICreateImage image)
        {
            this.db = db;
            this.webHost = webHost;
            this.image = image;
        }


        public async Task<IActionResult> Requests()
        {
            var result = await db.ClientForm.Where(x => x.isActive && !x.isAproved).ToListAsync();
            var test = result.Adapt<IEnumerable<ClientFormDto>>();
            return View(test);
        }

        public async Task<IActionResult> Clients()
        {
            var result = await db.ClientForm.Where(x => x.isActive && x.isAproved).ToListAsync();
            var test = result.Adapt<IEnumerable<ClientFormDto>>();
            return View(test);
        }
        [HttpGet]
        public IActionResult UserInitForm()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> UserInitForm(ClientFormDto obj)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    if (obj.LogoImage != null)
                    {
                        string serverPath = webHost.WebRootPath.ToString();
                        var img = image.ImagesCRa("FormLogo", obj.LogoImage, serverPath);
                        obj.Logo = img;
                    } else
                    {
                        obj.Logo = "Nologo.jfif";
                    }

                    var form = obj.Adapt<ClientForm>();
                    await db.ClientForm.AddAsync(form);
                    db.SaveChanges();
                    ViewBag.success = "Form Submitied Successfully";
                    ModelState.Clear();
                    return View();
                }
                else
                {
                    ModelState.AddModelError("", "Something Went Rong");
                    return View();
                }
            }
            catch (Exception)
            {

                throw;
            }

        }

        [HttpGet]
        public async Task<IActionResult> Approve(int id)
        {
            var result = await db.ClientForm.FindAsync(id);
            if (result != null)
            {
                result.isAproved = true;
                result.AproveDate = DateTime.Now;
                db.Update(result);
                db.SaveChanges();
                return RedirectToAction("UserInitForm");
            }
            return RedirectToAction("UserInitForm");
        }
        [HttpGet]
        public async Task<IActionResult> Details(int id)
        {
            try
            {
                var result = await db.ClientForm.FindAsync(id);
                if (result != null)
                {
                    var test = result.Adapt<ClientFormDto>();
                    return View(test);
                }
                else
                {

                    return View("Record you are looking for doesnot Exist or Removed from System");
                }
            }
            catch (Exception)
            {

                throw;
            }
        }
    }
}
