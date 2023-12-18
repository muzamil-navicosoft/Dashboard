using Dashboard.DataAccess.UnitOfWork;
using Dashboard.Mapping;
using Dashboard.Models.DTO;
using Dashboard.Models.Models;
using Humanizer;
using Mapster;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.EntityFrameworkCore;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;
using QuestPDF.Previewer;
using Serilog;

namespace Dashboard.Controllers
{
    public class BillingController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IHelper _helper;
        private readonly IWebHostEnvironment webHost;

        public BillingController(IUnitOfWork unitOfWork, IHelper helper, IWebHostEnvironment webHost)
        {
            _unitOfWork = unitOfWork;
            _helper = helper;
            this.webHost = webHost;
            _helper.ConfigureMapster();
            QuestPDF.Settings.License = LicenseType.Community;
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
                    if (result.IsPaid)
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
            catch (Exception e)
            {
                Log.Information($" location : {e.StackTrace} \n");
                Log.Information($" Error Message : {e.Message} \n");
                Log.Information($" Ineer Exeption : {e.Message}  \n");
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
            catch (Exception e)
            {
                Log.Information($" location : {e.StackTrace} \n");
                Log.Information($" Error Message : {e.Message} \n");
                Log.Information($" Ineer Exeption : {e.Message}  \n");
                throw;
            }
        }
        public IActionResult Index()
        {
            //var result =  _unitOfWork.billing.GetAll();
            var result = _unitOfWork.billing.CustomeGetAll().Include(x => x.ClientForm).Where(x => !x.IsPaid && x.ClientForm.isActive);
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
            if (result != null)
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
                if (test.IsPaid)
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
        public IActionResult ClientTicket(int id)
        {
            var result = _unitOfWork.ticket.CustomeGetAll().Include(x => x.ClientForm).Where(x => x.ClientFormId == 5).ToList();
            var test = result.Adapt<IEnumerable<TicketDto>>();
            return View(test);
        }

        public async Task<IActionResult> GenratePdf(int id)
        {
            var result = await _unitOfWork.billing.CustomeGetAll().Where(x => x.Id == id).Include( x => x.ClientForm).ToListAsync();
            if (result != null)
            {
                //var user = _unitOfWork.User.Get(result.ClientFormId);
                // code in your main method
                Document.Create(container =>
                {
                    var titleStyle = TextStyle.Default.FontSize(12).SemiBold().FontColor(Colors.Blue.Medium);
                    var Invoice = TextStyle.Default.FontSize(12);

                    
                    container.Page(page =>
                    {
                        // Defining Page Size

                        page.Size(PageSizes.A4);
                        page.Margin(50);
                        page.PageColor(Colors.White);
                        page.DefaultTextStyle(x => x.FontSize(16));


                        // Getting the root path for Image
                        string serverPath = webHost.WebRootPath.ToString();

                        page.Header().PaddingVertical(20).Container().Row(row =>
                        {
                            row.ConstantItem(200).Image($"{serverPath}/images/logo.png");
                            
                        });
                    
                        page.Content().PaddingVertical(20).Column(col =>
                        {
                            // For Company Name and Details
                            col.Item().AlignRight().Text("Navicosoft Pty Ltd").FontSize(18).Bold();
                            col.Item().AlignRight().Text("ABN: 19648285894").FontSize(12);
                            col.Item().AlignRight().Text("Tower 5/727 Collins St, Docklands VIC 3008").FontSize(12);


                            // For InvoiceNumber and  Due Date
                            col.Item().PaddingTop(10).AlignLeft().Text($"Invoice # {result[0].Id}").FontSize(18).Bold();
                            col.Item().AlignLeft().Text($"Bill For Month : {result[0].Month}").Style(Invoice);
                            col.Item().AlignLeft().Text($"Due Date : {result[0].DueDate}").Style(Invoice);


                            // Invoiced To
                            col.Item().PaddingTop(10).AlignLeft().Text($"Invoiced To").FontSize(18).Bold();
                            col.Item().AlignLeft().Text($"{result[0].ClientForm?.Name}").Style(Invoice);


                            // Table
                            col.Item().PaddingVertical(20).Table(table => 
                            {                           
                                table.ColumnsDefinition(columns =>
                                {
                                    columns.RelativeColumn();
                                    columns.RelativeColumn();
                                    //columns.RelativeColumn();
                                    //columns.RelativeColumn();
                                    //columns.RelativeColumn();
                                });

                                // step 2
                                table.Header(header =>
                                {
                                    //header.Cell().Element(CellStyle).Text("#");
                                    //header.Cell().Element(CellStyle).Text("Product");
                                    //header.Cell().Element(CellStyle).AlignRight().Text("Unit price");
                                    header.Cell().Element(CellStyle).AlignCenter().Text("Quantity");
                                    header.Cell().Element(CellStyle).AlignRight().Text("Total");

                                    static IContainer CellStyle(IContainer container)
                                    {
                                        return container.DefaultTextStyle(x => x.SemiBold()).PaddingVertical(5).BorderBottom(1).BorderColor(Colors.Black);
                                    }
                                });

                                //foreach (var item in result)
                                //{
                                //    table.Cell().Element(CellStyle).Text(Model.Items.IndexOf(item) + 1);
                                //    table.Cell().Element(CellStyle).Text(item.Name);
                                //    table.Cell().Element(CellStyle).AlignRight().Text($"{item.Price}$");
                                //    table.Cell().Element(CellStyle).AlignRight().Text(item.Quantity);
                                //    table.Cell().Element(CellStyle).AlignRight().Text($"{item.Price * item.Quantity}$");

                                //    static IContainer CellStyle(IContainer container)
                                //    {
                                //        return container.BorderBottom(1).BorderColor(Colors.Grey.Lighten2).PaddingVertical(5);
                                //    }
                                //}

                            });

                        });
                        
                        //page.Content().PaddingVertical(50).Container().Row(row =>
                        //{
                        //    row.ConstantItem(10, Unit.Millimetre).AlignRight().Text("Navicosoft Pty Ltd").FontSize(18).Bold();
                        //    row.ConstantItem(170, Unit.Millimetre).AlignRight().Text("Navicosoft Pty Ltd").FontSize(18).Bold();
                        //    //row.ConstantItem(170, Unit.Millimetre).AlignRight().Text("Navicosoft Pty Ltd").FontSize(18).Bold();
                        //    //row.RelativeItem().AlignRight().Text("ABN: 19648285894").FontSize(12);
                        //    //row.RelativeItem().AlignRight().Text("Tower 5/727 Collins St, Docklands VIC 3008").FontSize(12);
                        //    //row.RelativeItem().Column(column =>
                        //    //{
                        //    //    column.Item().AlignRight().Text("Navicosoft Pty Ltd").FontSize(18).Bold();
                        //    //    column.Item().AlignRight().Text("ABN: 19648285894").FontSize(12);
                        //    //    column.Item().AlignRight().Text("Tower 5/727 Collins St, Docklands VIC 3008").FontSize(12);
                        //    //});
                        //    //row.RelativeItem().Table(table => 
                        //    //{
                        //    //    table.ColumnsDefinition(columns =>
                        //    //    {
                        //    //        //columns.ConstantColumn(25);
                        //    //        //columns.RelativeColumn(3);
                        //    //        //columns.RelativeColumn();
                        //    //        columns.RelativeColumn();
                        //    //        columns.RelativeColumn();
                        //    //    });

                        //    //    // step 2
                        //    //    table.Header(header =>
                        //    //    {
                        //    //        //header.Cell().Element(CellStyle).Text("#");
                        //    //        //header.Cell().Element(CellStyle).Text("Product");
                        //    //        //header.Cell().Element(CellStyle).AlignRight().Text("Unit price");
                        //    //        header.Cell().Element(CellStyle).AlignCenter().Text("Quantity");
                        //    //        header.Cell().Element(CellStyle).AlignRight().Text("Total");

                        //    //        static IContainer CellStyle(IContainer container)
                        //    //        {
                        //    //            return container.DefaultTextStyle(x => x.SemiBold()).PaddingVertical(5).BorderBottom(1).BorderColor(Colors.Black);
                        //    //        }
                        //    //    });

                        //    //});
                           

                        //});
                        //page.Content().PaddingVertical(50).Container().Row(row =>
                        //{
                        //    //row.ConstantItem(170, Unit.Millimetre).AlignRight().Text("Navicosoft Pty Ltd").FontSize(18).Bold();
                        //    //row.ConstantItem(170, Unit.Millimetre).AlignRight().Text("Navicosoft Pty Ltd").FontSize(18).Bold();
                        //    //row.ConstantItem(170, Unit.Millimetre).AlignRight().Text("Navicosoft Pty Ltd").FontSize(18).Bold();
                        //    //row.RelativeItem().AlignRight().Text("ABN: 19648285894").FontSize(12);
                        //    //row.RelativeItem().AlignRight().Text("Tower 5/727 Collins St, Docklands VIC 3008").FontSize(12);
                        //    //row.RelativeItem().Column(column =>
                        //    //{
                        //    //    column.Item().AlignRight().Text("Navicosoft Pty Ltd").FontSize(18).Bold();
                        //    //    column.Item().AlignRight().Text("ABN: 19648285894").FontSize(12);
                        //    //    column.Item().AlignRight().Text("Tower 5/727 Collins St, Docklands VIC 3008").FontSize(12);
                        //    //});
                        //    row.RelativeItem().Table(table =>
                        //    {
                        //        table.ColumnsDefinition(columns =>
                        //        {
                        //            //columns.ConstantColumn(25);
                        //            //columns.RelativeColumn(3);
                        //            //columns.RelativeColumn();
                        //            columns.RelativeColumn();
                        //            columns.RelativeColumn();
                        //        });

                        //        // step 2
                        //        table.Header(header =>
                        //        {
                        //            //header.Cell().Element(CellStyle).Text("#");
                        //            //header.Cell().Element(CellStyle).Text("Product");
                        //            //header.Cell().Element(CellStyle).AlignRight().Text("Unit price");
                        //            header.Cell().Element(CellStyle).AlignCenter().Text("Quantity");
                        //            header.Cell().Element(CellStyle).AlignRight().Text("Total");

                        //            static IContainer CellStyle(IContainer container)
                        //            {
                        //                return container.DefaultTextStyle(x => x.SemiBold()).PaddingVertical(5).BorderBottom(1).BorderColor(Colors.Black);
                        //            }
                        //        });

                        //    });


                        //});


                        //page.Content().Table(table =>
                        //{

                        //    table.ColumnsDefinition(columns =>
                        //    {
                        //        //columns.ConstantColumn(25);
                        //        //columns.RelativeColumn(3);
                        //        //columns.RelativeColumn();
                        //        columns.RelativeColumn();
                        //        columns.RelativeColumn();
                        //    });

                        //    // step 2
                        //    table.Header(header =>
                        //    {
                        //        //header.Cell().Element(CellStyle).Text("#");
                        //        //header.Cell().Element(CellStyle).Text("Product");
                        //        //header.Cell().Element(CellStyle).AlignRight().Text("Unit price");
                        //        header.Cell().Element(CellStyle).AlignCenter().Text("Quantity");
                        //        header.Cell().Element(CellStyle).AlignRight().Text("Total");

                        //        static IContainer CellStyle(IContainer container)
                        //        {
                        //            return container.DefaultTextStyle(x => x.SemiBold()).PaddingVertical(5).BorderBottom(1).BorderColor(Colors.Black);
                        //        }
                        //    });

                        //    // step 3
                        //    //foreach (var item in Model.Items)
                        //    //{
                        //    //    table.Cell().Element(CellStyle).Text(Model.Items.IndexOf(item) + 1);
                        //    //    table.Cell().Element(CellStyle).Text(item.Name);
                        //    //    table.Cell().Element(CellStyle).AlignRight().Text($"{item.Price}$");
                        //    //    table.Cell().Element(CellStyle).AlignRight().Text(item.Quantity);
                        //    //    table.Cell().Element(CellStyle).AlignRight().Text($"{item.Price * item.Quantity}$");

                        //    //    static IContainer CellStyle(IContainer container)
                        //    //    {
                        //    //        return container.BorderBottom(1).BorderColor(Colors.Grey.Lighten2).PaddingVertical(5);
                        //    //    }
                        //    //}

                        //});


                        //string serverPath = webHost.WebRootPath.ToString();

                        //page.Header()
                        //    .Width(200)
                        //    .Image($"{serverPath}/Images/logo.png");


                        //page.Content()
                        //    .PaddingVertical(1, Unit.Centimetre)
                        //    .Column(x =>
                        //    {
                        //        x.Spacing(20);

                        //        x.Item().Text(Placeholders.LoremIpsum());
                        //        x.Item().Image(Placeholders.Image(200, 100));
                        //    });

                        page.Footer()
                            .AlignCenter()
                            .Text(x =>
                            {
                                x.Span($"PDF Generated on {DateTime.Now.DayOfWeek}, {DateTime.Now.Day},{DateTime.Now.Year}");

                            });
                    });
                })
                .GeneratePdfAndShow();
                return RedirectToAction("index");

            }
            return RedirectToAction("index");
        }

        void ComposeHeader(IContainer container)
        {
            var titleStyle = TextStyle.Default.FontSize(20).SemiBold().FontColor(Colors.Blue.Medium);

            container.Row(row =>
            {
                string serverPath = webHost.WebRootPath.ToString();
                row.ConstantItem(200).Image($"{serverPath}/images/logo.png");
                row.RelativeItem().Column(column =>
                {
                    column.Item().Text($"Invoice #{Placeholders.Integer}").Style(titleStyle);

                    column.Item().Text(text =>
                    {
                        text.Span("Issue date: ").SemiBold();
                        text.Span($"{Placeholders.DateTime:d}");
                    });

                    column.Item().Text(text =>
                    {
                        text.Span("Due date: ").SemiBold();
                        text.Span($"{Placeholders.DateTime:d}");
                    });
                });

               
            });
        }



    }
}
