using Dashboard.Models.DTO;
using Dashboard.Utillities.Helper;
using Dashboard.Models.Models;
using Mapster;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Dashboard.DataAccess.UnitOfWork;
using Dashboard.Mapping;
using Microsoft.Data.SqlClient;
using System.Text;
using Dashboard.Utillities.Helper.Email;
using Hangfire;
using Microsoft.AspNetCore.Authorization;

namespace Dashboard.Controllers
{
    //[Authorize(Roles = "Admin")]
    public class UserFormController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IWebHostEnvironment webHost;
        private readonly ICreateImage image;
        private readonly IConfiguration configuration;
        private readonly IEmailService emailService;

        public IHelper Helper { get; }

        public UserFormController(IUnitOfWork unitOfWork, 
                        IWebHostEnvironment webHost, 
                        ICreateImage image, 
                        IHelper helper,
                        IConfiguration configuration,IEmailService emailService)
        {
            this._unitOfWork = unitOfWork;
            this.webHost = webHost;
            this.image = image;
            Helper = helper;
            this.configuration = configuration;
            this.emailService = emailService;
            Helper.ConfigureMapster();
        }

        [HttpGet]
        public async Task<IActionResult> Requests()
        {
            var result = await _unitOfWork.User
                                .CustomeGetAll()
                                .Where(x => x.isActive && !x.isAproved)
                                .AsNoTracking()
                                .ToListAsync();

            var test = result.Adapt<IEnumerable<ClientFormDto>>();
            return View(test);
        }

        [HttpGet]
        public async Task<IActionResult> Clients()
        {
            try
            {
                var result = await _unitOfWork.User
                                   .CustomeGetAll()
                                   .Include(x => x.Tickets)
                                   .AsNoTracking()
                                   .Where(x => x.isActive && x.isAproved && !x.isDeleted)
                                   .ToListAsync();

                var test = result.Adapt<IEnumerable<ClientFormDto>>();
                return View(test);
            }
            catch (Exception e)
            {

                throw;
            }
           
        }

        [HttpGet]
        public async Task<IActionResult> DeactiveClients()
        {
            var result = await _unitOfWork.User
                                .CustomeGetAll()
                                .Where(x => !x.isActive && x.isAproved && !x.isDeleted)
                                .ToListAsync();

            var test = result.Adapt<IEnumerable<ClientFormDto>>();
            return View(test);
        }

        [HttpGet]
        public IActionResult UserInitForm()
        {
            return View();
        }

        [HttpPost]
        public IActionResult UserInitForm(ClientFormDto obj)
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
        public async Task<IActionResult> AddBill(int id)
        {
            var result = await _unitOfWork.User.CustomeGetAll().Where(x => x.Id == id).AsNoTracking().FirstOrDefaultAsync();
            if (result != null)
            {
                var test = result.Adapt<ClientFormDto>();
                return View(test);
            }
            else
            {
                ViewBag.NotFound = "Not Found "; 
                return View();
            }
        }
        [HttpPost]
        public IActionResult AddBill(ClientFormDto obj)
        {
            var result = _unitOfWork.User.CustomeGetAll().Where(x => x.Id == obj.Id).AsNoTracking().FirstOrDefault();
            if (result != null)
            {
                result.OneTimeBill = obj.OneTimeBill;
                _unitOfWork.User.update(result);
                _unitOfWork.Save();
                return RedirectToAction("Requests");
            }
            else
            {
                ViewBag.NotFound = "Not Found ";
                return View();
            }

        }


        [HttpGet]
        public async Task<IActionResult> Approve(int id)
        {
            var result =  _unitOfWork.User.Get(id);
           
           // var testing = billingresult.DueDate.Day;


            if (result != null)
            {
                result.isAproved = true;
                result.AproveDate = DateTime.Now;
                //var server = configuration.GetSection("Database:server").Value;
                var dbName = result.Name.Split(' ')[0];
                var NewDb = configuration.GetSection("Database:Newconnection").Value;
                SqlConnection Newconnection = new SqlConnection(NewDb);
                Newconnection.Open();

                SqlCommand command = new SqlCommand("CREATE DATABASE " + dbName, Newconnection);
                command.ExecuteNonQuery();

                string scriptFilePath = Path.Combine(webHost.WebRootPath.ToString(),"Tables.sql");
              

                string script = System.IO.File.ReadAllText(scriptFilePath);

                var dbServer = configuration.GetSection("Database:DbServer").Value;
                var dbUser = configuration.GetSection("Database:DbUser").Value;
                var dbPassword = configuration.GetSection("Database:DbPassword").Value;
                var dbCertificate = configuration.GetSection("Database:Dbcertificate").Value;

                StringBuilder conStr = new StringBuilder();
                
                conStr.Append("Server =");
                conStr.Append(dbServer);
                conStr.Append(";Database =");
                conStr.Append(dbName);
                conStr.Append(";User Id =");
                conStr.Append(dbUser);
                conStr.Append(";Password =");
                conStr.Append(dbPassword);
                conStr.Append(";TrustServerCertificate =");
                conStr.Append(dbCertificate);

                var newConStr =  conStr.ToString();

                
                // Create a command and execute each SQL statement
                string[] sqlStatements = script.Split(new[] { "GO\r\n", "GO ", "GO\t" }, 
                                                    StringSplitOptions.RemoveEmptyEntries);
                using (var cmd = new SqlCommand())
                {
                    SqlConnection NewDBconnection = new SqlConnection(newConStr);
                    NewDBconnection.Open();
                    cmd.Connection = NewDBconnection;

                    foreach (var sqlStatement in sqlStatements)
                    {
                        cmd.CommandText = sqlStatement;
                        cmd.ExecuteNonQuery();
                    }
                }

                Newconnection.Close();
                // Adding Bill Scedulling 

                //if (result.isBilledMonthly.Value == true)
                //{
                //    var resultbill = billingTask(id);
                    
                //    //RecurringJob.AddOrUpdate(() => AddBill(resultbill), Cron.MinuteInterval(5));
                //    RecurringJob.AddOrUpdate("add-bill-job", () => AddBill(resultbill), Cron.MinuteInterval(5));

                //}


                // Adding API call here 
                var test = await GenralPurpose.SendPostRequestAsync();

                var test2 = await GenralPurpose.SendPostSubDomainCreateRequestAsync(test, dbName);
                emailService.SendEmail(result.Email, "Welcome to NavicoSoft", "WelCome Email");
                Console.WriteLine(test2);
                result.SubDomain = dbName+ ".navedge.co";
                _unitOfWork.User.update(result);
                _unitOfWork.Save();
                return RedirectToAction("Clients");
            }
            return RedirectToAction("Clients");
        }

        [HttpGet]
        public IActionResult Discontinue(int id)
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
        public IActionResult Details(int id)
        {
            try
            {
                var result = _unitOfWork.User.CustomeGetAll()
                             .Include(t => t.Tickets)
                             .Include(b => b.BillingInfos)
                             .AsSplitQuery()
                             .AsNoTracking()
                             .Where(x => x.Id == id).FirstOrDefault();
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
        public IActionResult UserDetail()
        {
            try
            {
                var userEmail = User.FindFirst("userEmail")?.Value;
                var result = _unitOfWork.User.CustomeGetAll()
                             .Include(t => t.Tickets)
                             .Include(b => b.BillingInfos)
                             .AsSplitQuery()
                             .AsNoTracking()
                             .Where(x => x.Email == userEmail).FirstOrDefault();
                //var result =  _unitOfWork.User.Get(id);
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

        [HttpGet]
        public IActionResult Edit (int id)
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


        private BillingInfo billingTask(int id)
        {
            var billingresult = _unitOfWork.billing.CustomeGetAll().Where(x => x.ClientFormId == id).FirstOrDefault();
            
            return billingresult;
        }
        public void AddBill(BillingInfo obj)
        {
            obj.DueDate = obj.DueDate.AddMonths(1);
            obj.Month = obj.DueDate.ToString("MMMM");

            _unitOfWork.billing.Add(obj);
            _unitOfWork.Save();
        }

        // Ajax Function for Check box
        [HttpPost]
        public void updateIsBilledMonthly(int id, bool billMonthly)
        {
            var result = _unitOfWork.User.CustomeGetAll().Where(x => x.Id == id).AsNoTracking().FirstOrDefault();
            if(result != null)
            {
                var test = result.Adapt<ClientForm>();
                if( !billMonthly )
                {
                    test.OneTimeBill = null;
                }
                test.isBilledMonthly = billMonthly;
                _unitOfWork.User.update(test);
                _unitOfWork.Save();
            }
        }
    }
}
