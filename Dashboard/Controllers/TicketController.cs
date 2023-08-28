using Dashboard.DataAccess.UnitOfWork;
using Dashboard.Mapping;
using Dashboard.Models.DTO;
using Dashboard.Models.Models;
using Mapster;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

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
            var user = _unitOfWork.User.GetAll();
            ViewBag.Subdomain = new SelectList(user, "Id", "Email");
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
            var result = _unitOfWork.ticket.CustomeGetAll().Include(x => x.ClientForm).Where(x => x.IsActive);
            var test = result.Adapt<IEnumerable<TicketDto>>();
            return View(test);
        }
        public IActionResult Closed()
        {
            //var result =  _unitOfWork.ticket.GetAll();
            var result = _unitOfWork.ticket.CustomeGetAll().Include(x => x.ClientForm).Where(x => !x.IsActive);
            var test = result.Adapt<IEnumerable<TicketDto>>();
            return View(test);
        }
        public IActionResult Resolve(int id)
        {
            var result = _unitOfWork.ticket.Get(id);
            if (result != null)
            {
                result.IsActive = false;
                result.DateReolved = DateTime.Now;
                _unitOfWork.ticket.update(result);
                _unitOfWork.Save();
                return RedirectToAction("Index");
            }
            return RedirectToAction("Index");
        }


    }
}
