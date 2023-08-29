using Dashboard.Data;
using Dashboard.Models.DTO;
using Dashboard.Utillities.Helper;
using Dashboard.Models.Models;
using Mapster;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Dashboard.DataAccess.UnitOfWork;

namespace Dashboard.Controllers
{
    public class UserForm : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IWebHostEnvironment webHost;
        private readonly ICreateImage image;

        public UserForm(IUnitOfWork unitOfWork, IWebHostEnvironment webHost, ICreateImage image)
        {
            this._unitOfWork = unitOfWork;
            this.webHost = webHost;
            this.image = image;
        }


        public async Task<IActionResult> Requests()
        {
            var result = await _unitOfWork.User.CustomeGetAll().Where(x => x.isActive && !x.isAproved).ToListAsync();
            var test = result.Adapt<IEnumerable<ClientFormDto>>();
            return View(test);
        }

        public async Task<IActionResult> Clients()
        {
            try
            {
                var result = await _unitOfWork.User.CustomeGetAll().Include(x => x.Tickets).AsNoTracking().Where(x => x.isActive && x.isAproved && !x.isDeleted).ToListAsync();
                var test = result.Adapt<IEnumerable<ClientFormDto>>();
                return View(test);
            }
            catch (Exception e)
            {

                throw;
            }
           
        }
        public async Task<IActionResult> DeactiveClients()
        {
            var result = await _unitOfWork.User.CustomeGetAll().Where(x => !x.isActive && x.isAproved && !x.isDeleted).ToListAsync();
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
                    _unitOfWork.User.Add(form);
                    _unitOfWork.Save();
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
            var result =  _unitOfWork.User.Get(id);
            if (result != null)
            {
                result.isAproved = true;
                result.AproveDate = DateTime.Now;
                _unitOfWork.User.update(result);
                _unitOfWork.Save();
                return RedirectToAction("Clients");
            }
            return RedirectToAction("Clients");
        }

        [HttpGet]
        public async Task<IActionResult> Discontinue(int id)
        {
            var result =  _unitOfWork.User.Get(id);
            if (result != null)
            {
                result.isActive = false;
                result.DiscontinueDate = DateTime.Now;
                _unitOfWork.User.update(result);
                _unitOfWork.Save();
                return RedirectToAction("UserInitForm");
            }
            return RedirectToAction("UserInitForm");
        }
        [HttpGet]
        public async Task<IActionResult> Details(int id)
        {
            try
            {
                var result = _unitOfWork.User.CustomeGetAll().Include(x => x.Tickets).AsNoTracking().Where(x => x.Id == id).FirstOrDefault();
                //var result =  _unitOfWork.User.Get(id);
                if (result != null)
                {
                    var test = result.Adapt<ClientFormDto>();
                    return View(test);
                }
                else
                {
                    ViewBag.NotFound= "Record you are looking for doesnot Exist or Removed from System";
                    return View();
                }
            }
            catch (Exception)
            {

                throw;
            }
        }

        [HttpGet]
        public async Task<IActionResult> Edit (int id)
        {
            try
            {
                var result =  _unitOfWork.User.Get(id);
                
                if (result != null)
                {
                    var test = result.Adapt<ClientFormDto>();
                    return View(test);
                }
                else
                {
                    ViewBag.NotFound = "Record you are looking for doesnot Exist or Removed from System";
                    return View();
                }
            }
            catch (Exception)
            {

                throw;
            }
        }

        [HttpPost]
        public async Task<IActionResult> Edit(ClientFormDto obj)
        {
            try
            {
                var result =  await _unitOfWork.User.CustomeGetAll().AsNoTracking().FirstOrDefaultAsync(x => x.Id == obj.Id);
                if (result != null)
                {
                    result = obj.Adapt<ClientForm>();
                   // db.ClientForm.Attach(result);
                    _unitOfWork.User.update(result);
                    _unitOfWork.Save();
                    
                    return RedirectToAction("Clients");
                }
                else
                {
                    ViewBag.NotFound = "Record you are looking for doesnot Exist or Removed from System";
                    return View();
                }
            }
            catch (Exception)
            {

                throw;
            }
        }
    }
}
