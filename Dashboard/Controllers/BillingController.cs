using Dashboard.DataAccess.UnitOfWork;
using Dashboard.Mapping;
using Dashboard.Models.DTO;
using Dashboard.Models.Models;
using Humanizer;
using Mapster;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.EntityFrameworkCore;

namespace Dashboard.Controllers
{
    public class BillingController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IHelper _helper;
        public BillingController(IUnitOfWork unitOfWork, IHelper helper)
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
        public IActionResult Add(BillingInfoDto obj)
        {
           
            try
            {
                if (ModelState.IsValid)
                {
                    var result = obj.Adapt<BillingInfo>();       
                    if( result.IsPaid)
                    {
                        result.PaidDate = DateTime.Now;
                    }
                    _unitOfWork.billing.Add(result);
                    _unitOfWork.Save();
                    ViewBag.success = "Form Submitied Successfully";
                    ModelState.Clear();
                    var user = _unitOfWork.User.GetAll();
                    ViewBag.Subdomain = new SelectList(user, "Id", "Email");
                    return View();
                }
                else
                {
                    ModelState.AddModelError("", "Something Went Rong");
                    var user = _unitOfWork.User.GetAll();
                    ViewBag.Subdomain = new SelectList(user, "Id", "Email");
                    return View();
                }
            }
            catch (Exception)
            {
                throw;
            }
        }
        [HttpGet]
        public IActionResult AddBill(int ClientFormId)
        {
            return View();
        }
        [HttpPost]
        public IActionResult AddBill(BillingInfoDto obj)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var result = obj.Adapt<BillingInfo>();
                    result.Month = result.DueDate.ToString("MMMM");
                    _unitOfWork.billing.Add(result);
                    _unitOfWork.Save();

                    return RedirectToAction("Requests", "UserForm");
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
            //var result =  _unitOfWork.billing.GetAll();
            var result =  _unitOfWork.billing.CustomeGetAll().Include(x=>x.ClientForm).Where(x=> !x.IsPaid && x.ClientForm.isActive);
            //var result = _unitOfWork.ticket.CustomeGetAll().Include(x => x.ClientForm).Where(x => x.IsActive);
            var test = result.Adapt<IEnumerable<BillingInfoDto>>();
            return View(test);
        }
        // For Month Specific Filter
        public IActionResult BillAcordingToMonth(string month)
        {
            
            var result = _unitOfWork.billing.CustomeGetAll().Include(x => x.ClientForm)
                                    .Where(x => !x.IsPaid && x.Month == month && x.ClientForm.isActive);
           
            var test = result.Adapt<IEnumerable<BillingInfoDto>>();
            return View(test);
        }

        [HttpGet]
        public IActionResult Edit(int id)
        {
            var result = _unitOfWork.billing.Get(id);
            if(result != null)
            {
                var result2 = result.Adapt<BillingInfoDto>();
                return View(result2);
            }
            return View();
        }
        public IActionResult Edit(BillingInfoDto obj)
        {
            if (ModelState.IsValid)
            {
                var test = obj.Adapt<BillingInfo>();
                if(test.IsPaid)
                {
                    test.PaidDate = DateTime.Now;
                }
                _unitOfWork.billing.update(test);
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
        public async Task<IActionResult> Resolve(int id)
        {
            try
            {
                var result = await _unitOfWork.ticket.CustomeGetAll().Include(x => x.ClientForm).AsNoTracking().Where(x => x.Id == id).FirstOrDefaultAsync();

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
