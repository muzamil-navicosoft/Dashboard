using Dashboard.Data;
using Dashboard.DTO;
using Dashboard.Helper;
using Dashboard.Models;
using Mapster;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Dashboard.Controllers
{
    public class MainController : Controller
    {
        private readonly ProjectContext db;
        private readonly IWebHostEnvironment webHost;
        private readonly ICreateImage imageHelper;

        public MainController(ProjectContext db, IWebHostEnvironment webHost, ICreateImage imageHelper)
        {
            this.db = db;
            this.webHost = webHost;
            this.imageHelper = imageHelper;
        }



        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var result = await db.Clients.Where(x => x.IsActive).ToListAsync();
            var list = result.Adapt<IEnumerable<ClientsDto>>();
          
            return View(list);
        }

        [HttpGet]
        public async Task<IActionResult> Inactive()
        {
            var result = await db.Clients.Where(x => !x.IsActive).ToListAsync();
            var list = result.Adapt<IEnumerable<ClientsDto>>();
            return View(list);
        }

        [HttpGet]
        public async Task<IActionResult> GetById(int id)
        {
            var result = await db.Clients.FindAsync(id);
            var test = result?.Adapt<ClientsDto>();
            return View(test);
        }
        [HttpGet]
        public IActionResult AddClient()
        {
            return View();
        }


        [HttpPost]
        public async Task<IActionResult> AddClient(ClientsDto obj)
        {
            try
            {
                if (ModelState.IsValid)
                {

                    if (obj.LogoImage != null)
                    {
                        string serverPath = webHost.WebRootPath.ToString();
                        var img = imageHelper.ImagesCRa("Logo", obj.LogoImage, serverPath);

                        //string ImagesName =  Guid.NewGuid().ToString()+"_"+obj.LogoImage.FileName;
                        //string Folder = "Images/Logo/";
                        //Folder += ImagesName;
                        //string ServerPath = Path.Combine(webHost.WebRootPath, Folder);

                        //await obj.LogoImage.CopyToAsync(new FileStream(ServerPath, FileMode.Create));

                        obj.Logo = img;

                    }
                    else
                    {
                        obj.Logo = "Nologo.jfif";
                    }
                    var client = obj.Adapt<Clients>();
                    await db.Clients.AddAsync(client);
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
                else
                {
                    ModelState.AddModelError("", "asdfa");
                    return View(ModelState);
                }


            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

    }
}
