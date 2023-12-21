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
using System.Diagnostics;
using Microsoft.Extensions.FileProviders.Composite;
using Serilog;
using Dashboard.Data;

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
            try
            {
                var result = await _unitOfWork.User
                               .CustomeGetAll()
                               .Where(x => x.isActive && !x.isAproved)
                               .AsNoTracking()
                               .ToListAsync();

                var test = result.Adapt<IEnumerable<ClientFormDto>>();
                return View(test);
            }
            catch (Exception e)
            {
                Log.Information($" location : {e.StackTrace} \n");
                Log.Information($" Error Message : {e.Message} \n");
                Log.Information($" Ineer Exeption : {e.Message}  \n");
                return RedirectToAction("wentWrong","Home");
            }
           
        }

        [HttpGet]
        public async Task<IActionResult> Clients()
        {
            try
            {
                var username = User?.Identity?.Name;
                Log.Information($" loged in user is {username}");
                Log.Information($"  Request come here after redirection ");

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
                Log.Information($" location : {e.StackTrace} \n");
                Log.Information($" Error Message : {e.Message} \n");
                Log.Information($" Ineer Exeption : {e.Message}  \n");
                return RedirectToAction("wentWrong", "Home");
            }
           
        }

        // For Paggingnattion
        [HttpPost]
        public async Task<IActionResult> GetCustomers()
        {
            try
            {
                var result = await _unitOfWork.User
                                   .CustomeGetAll()
                                   .Include(x => x.Tickets)
                                   .AsNoTracking()
                                   .Where(x => x.isActive && x.isAproved && !x.isDeleted)
                                   .ToListAsync();

                var draw = Request.Form["draw"].FirstOrDefault();
                var start = Request.Form["start"].FirstOrDefault();
                var length = Request.Form["length"].FirstOrDefault();
                var sortColumn = Request.Form["columns[" + Request.Form["order[0][column]"].FirstOrDefault() + "][name]"].FirstOrDefault();
                var sortColumnDirection = Request.Form["order[0][dir]"].FirstOrDefault();
                var searchValue = Request.Form["search[value]"].FirstOrDefault();
                int pageSize = length != null ? Convert.ToInt32(length) : 0;
                int skip = start != null ? Convert.ToInt32(start) : 0;
                int recordsTotal = 0;
                //var customerData = (from tempcustomer in ProjectContext.User select tempcustomer);
                var customerData = await (from tempcustomer in _unitOfWork.User.CustomeGetAll()
                                    where tempcustomer.isActive && tempcustomer.isAproved && !tempcustomer.isDeleted
                                    select new { tempcustomer }).ToListAsync();
                //if (!(string.IsNullOrEmpty(sortColumn) && string.IsNullOrEmpty(sortColumnDirection)))
                //{
                //    customerData = customerData.OrderBy(sortColumn + " " + sortColumnDirection);
                //}
                if (!string.IsNullOrEmpty(searchValue))
                {
                    customerData = customerData.Where(m => m.tempcustomer.Name.Contains(searchValue)
                                                || m.tempcustomer.Email.Contains(searchValue)
                                                || m.tempcustomer.BlackBaudApiId.Contains(searchValue)).ToList();
                                                /*|| m.Email.Contains(searchValue))*/
                }
                recordsTotal = customerData.Count();
                var data = customerData.Skip(skip).Take(pageSize).ToList();
                var jsonData = new { draw = draw, recordsFiltered = recordsTotal, recordsTotal = recordsTotal, data = data };
                return Ok(jsonData);
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        [HttpGet]
        public async Task<IActionResult> DeactiveClients()
        {
            try
            {
                var result = await _unitOfWork.User
                                .CustomeGetAll()
                                .Where(x => !x.isActive && x.isAproved && !x.isDeleted)
                                .ToListAsync();

                var test = result.Adapt<IEnumerable<ClientFormDto>>();
                return View(test);
            }
            catch (Exception e)
            {
                Log.Information($" location : {e.StackTrace} \n");
                Log.Information($" Error Message : {e.Message} \n");
                Log.Information($" Ineer Exeption : {e.Message}  \n");
                return RedirectToAction("wentWrong", "Home");
            }
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
            catch (Exception e)
            {

                Log.Information($" location : {e.StackTrace} \n");
                Log.Information($" Error Message : {e.Message} \n");
                Log.Information($" Ineer Exeption : {e.Message}  \n");
                return RedirectToAction("wentWrong", "Home");
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

            try
            {
                if (result != null)
                {
                   
                    result.isAproved = true;
                    result.AproveDate = DateTime.Now;
                    var dbName = result.Name.Split(' ')[0];

                    #region database creation and Seeding

                    var username = User.Identity.Name;
                    Log.Information($" loged in user is {username}");
                    Log.Information($" Database Creation started");
                    // Getting the connection string for creating new database in server as master 
                    var NewDb = configuration.GetSection("Database:Newconnection").Value;
                    SqlConnection Newconnection = new SqlConnection(NewDb);
                    Newconnection.Open();

                    //Creating the new database with the firstname of enternd company name
                    SqlCommand command = new SqlCommand("CREATE DATABASE " + dbName, Newconnection);
                    command.ExecuteNonQuery();

                    //setting up database script file which is to be executed 
                    string scriptFilePath = Path.Combine(webHost.WebRootPath.ToString(), "Tables.sql");

                    // Reading database script file
                    string script = System.IO.File.ReadAllText(scriptFilePath);

                    var dbServer = configuration.GetSection("Database:DbServer").Value;
                    var dbUser = configuration.GetSection("Database:DbUser").Value;
                    var dbPassword = configuration.GetSection("Database:DbPassword").Value;
                    var dbCertificate = configuration.GetSection("Database:Dbcertificate").Value;
                    var domainName = configuration.GetSection("Server:domainName").Value;

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

                    var newConStr = conStr.ToString();


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

                    Log.Information($" Database Creation Ended");
                    #endregion

                    #region Creating Subdomain

                    
                    Log.Information($" loged in user is {username}");
                    Log.Information($" Subdomain Creation started");
                    // Adding API call here 

                    // Getting Auth key here 
                    var test = await GenralPurpose.SendPostRequestAsync();

                    // Creating Subdomain 
                    var test2 = await GenralPurpose.SendPostSubDomainCreateRequestAsync(test, dbName);

                    Log.Information($" Subdomain Creation Ended");

                    #endregion



                    #region copying the sample project files to the newly created subdomain

                    Log.Information($"  started Copy the sample project files ");

                    var rootfolder = webHost.WebRootPath.ToString();
                    var subdomain = dbName + "." + domainName;

                    var sourceFolder = rootfolder + "/sampleproject";
                    var destinationFolder = "C:\\inetpub\\vhosts\\navedge.co\\" + subdomain;

                    Process process = new Process();
                    process.StartInfo.FileName = "robocopy";
                    process.StartInfo.Arguments = $"\"{sourceFolder}\" \"{destinationFolder}\" /e";
                    process.StartInfo.UseShellExecute = false;
                    process.StartInfo.RedirectStandardOutput = true;
                    process.StartInfo.RedirectStandardError = true;
                    process.StartInfo.CreateNoWindow = true;

                    process.Start();

                    Log.Information($" loged in user is {username}");

                    Log.Information($"  Files Copied to the Destination Folders ");

                    //string output = process.StandardOutput.ReadToEnd();
                    //string error = process.StandardError.ReadToEnd();

                    //process.WaitForExit();

                    //if (process.ExitCode == 0)
                    //{
                    //    Log.Information($" Filles copied to the newly create subdomain is successfull ");
                    //}
                    //else
                    //{
                    //    Log.Information($" robocopy failed with error code , {process.ExitCode}");
                    //    Log.Information($" Standard Output: , {output}");
                    //    Log.Information($" Standard Error :, {error}");
                    //}

                    #endregion


                    //Sending Welcome Email to newly creating Company on there company email address mention in the form 
                    emailService.SendEmail(result.Email, "Welcome to NavicoSoft", "WelCome Email");
                    Log.Information($" loged in user is {username}");

                    Log.Information($"  Welcome Email Send to Newaly create Project USer ");


                    Log.Information($"This is before saving subdomin name in user table loged in user is {username}");
                    result.SubDomain = dbName + "."+domainName;
                    _unitOfWork.User.update(result);
                    _unitOfWork.Save();
                    Log.Information($"This is after saving subdomin name in user table loged in user is {username}");
                    return RedirectToAction("Clients");
                }
                return RedirectToAction("Clients");
            }
            catch (SqlException sqlEx)
            {
                Log.Error($"SQL Exception occurred: {sqlEx.Message}, Stack Trace: {sqlEx.StackTrace}");
                // Handle SQL related exceptions, maybe rollback changes or notify administrators
                // Redirect to an error page or return a specific error message
                return RedirectToAction("wentWrong", "Home");
            }
            catch (IOException ioEx)
            {
                Log.Error($"IO Exception occurred: {ioEx.Message}, Stack Trace: {ioEx.StackTrace}");
                // Handle file-related exceptions, log the error, and possibly rollback changes
                // Redirect to an error page or return a specific error message
                return RedirectToAction("wentWrong", "Home");
            }
            catch (HttpRequestException httpEx)
            {
                Log.Error($"HTTP Request Exception occurred: {httpEx.Message}, Stack Trace: {httpEx.StackTrace}");
                // Handle HTTP request related exceptions, log the error
                // Redirect to an error page or return a specific error message
                return RedirectToAction("wentWrong", "Home");
            }
            catch (Exception ex)
            {
                Log.Error($"An unexpected Exception occurred: {ex.Message}, Stack Trace: {ex.StackTrace}");
                // Log any other unexpected exceptions and handle them appropriately
                // Redirect to an error page or return a specific error message
                return RedirectToAction("wentWrong", "Home");
            }
         

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
