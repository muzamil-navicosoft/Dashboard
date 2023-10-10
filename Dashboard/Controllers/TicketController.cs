using Dashboard.DataAccess.Migrations;
using Dashboard.DataAccess.UnitOfWork;
using Dashboard.Mapping;
using Dashboard.Models.DTO;
using Dashboard.Models.Models;
using Mapster;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;

namespace Dashboard.Controllers
{
    public class TicketController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IHelper _helper;
        public TicketController(IUnitOfWork unitOfWork, IHelper helper)
        {
            _unitOfWork = unitOfWork;
            _helper = helper;
            _helper.ConfigureMapster();
        }
        [HttpGet]
        public IActionResult Add()
        {
            var test = User.IsInRole("Client");
            if (User.IsInRole("Admin"))
            {
                var user = _unitOfWork.User.GetAll();
                ViewBag.Subdomain = new SelectList(user, "Id", "Email");
            } 
            else if (User.IsInRole("Client"))
            {
                var userEmail = User.FindFirst("userEmail")?.Value;
                List<ClientForm> user = new List<ClientForm>();
                var result = _unitOfWork.User.CustomeGetAll().Where(x => x.Email == userEmail).FirstOrDefault();
                user.Add(result);
                ViewBag.Subdomain = new SelectList(user, "Id", "Email");
            }

            return View();
        }
        [HttpPost]
        public IActionResult Add(TicketDto obj)
        {

            try
            {
                if (ModelState.IsValid)
                {


                    var result = obj.Adapt<Ticket>();

                    _unitOfWork.ticket.Add(result);
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
        public IActionResult Index()
        {
            //var result =  _unitOfWork.ticket.GetAll();
            if (User.IsInRole("Admin")|| User.IsInRole("Tickitting"))
            {
                var result = _unitOfWork.ticket.CustomeGetAll().Include(x => x.ClientForm).Where(x => x.IsActive);
                var test = result.Adapt<IEnumerable<TicketDto>>();
                return View(test);
            }
            else if(User.IsInRole("Billing"))
            {
                var result = _unitOfWork.ticket.CustomeGetAll().Include(x => x.ClientForm).Where(x => x.IsActive && x.Department== "Billing");
                var test = result.Adapt<IEnumerable<TicketDto>>();
                return View(test);
            }
            return View(new List<TicketDto>());
        }

        [HttpGet]
        public IActionResult Edit(int id)
        {
            var result = _unitOfWork.ticket.Get(id);
            if (result != null)
            {
                var deparments = new List<string> { "Admin", "Tickitting", "Billing" };
                ViewBag.Departments = new SelectList(deparments);
                var result2 = result.Adapt<TicketDto>();
                return View(result2);
            }
            return View();
        }
        [HttpPost]
        public IActionResult Edit(TicketDto obj)
        {
            if (ModelState.IsValid)
            {
                var test = obj.Adapt<Ticket>();
               
                _unitOfWork.ticket.update(test);
                _unitOfWork.Save();
            }

            //var test = result.Adapt<IEnumerable<BillingInfoDto>>();
            return RedirectToAction("Index");
        }
        public IActionResult Closed()
        {
            //var result =  _unitOfWork.ticket.GetAll();
            var result = _unitOfWork.ticket.CustomeGetAll().Include(x => x.ClientForm).Where(x => !x.IsActive);
            var test = result.Adapt<IEnumerable<TicketDto>>();
            return View(test);
        }

        [HttpGet]
        public  IActionResult Resolve(int id)
        {
            try
            {
                var result = _unitOfWork.ticket.CustomeGetAll().Include(x => x.ClientForm).AsNoTracking().Where(x => x.Id == id).FirstOrDefault();

                if (result != null)
                {
                    
                    var test = result.Adapt<TicketDto>();
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
        public async Task<IActionResult> Resolve(TicketDto obj)
        {
            //var result = _unitOfWork.ticket.Get(id);
            var result = await _unitOfWork.ticket.CustomeGetAll().Include(x => x.ClientForm).AsNoTracking().FirstOrDefaultAsync(x => x.Id == obj.Id);
            if (result != null)
            {
                result.IsActive = false;
                result.DateReolved = DateTime.Now;
                result.Resolution = obj.Resolution;
                //obj.IsActive = false;
                //obj.DateReolved = DateTime.Now;
                //result = obj.Adapt<Ticket>();
                _unitOfWork.ticket.update(result);
                _unitOfWork.Save();
                return RedirectToAction("Index");
            }
            return RedirectToAction("Index");
        }

        [HttpGet]
        public  IActionResult ClientTicket(int id)
        {
            var result = _unitOfWork.ticket.CustomeGetAll().Include(x => x.ClientForm).Where( x => x.ClientFormId == 5).ToList();
            var test = result.Adapt<IEnumerable<TicketDto>>();
            return View(test);
        }


    }
}
