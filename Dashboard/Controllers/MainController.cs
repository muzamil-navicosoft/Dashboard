using Dashboard.Data;
using Dashboard.DTO;
using Dashboard.Models;
using Mapster;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Dashboard.Controllers
{
    public class MainController : Controller
    {
        private readonly ProjectContext db;

        public MainController(ProjectContext db)
        {
            this.db = db;
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
        public async Task<IActionResult> AddClient(Clients obj)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    await db.Clients.AddAsync(obj);
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
