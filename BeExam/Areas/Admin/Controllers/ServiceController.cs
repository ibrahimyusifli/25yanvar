using BeExam.Areas.Admin.ViewModels;
using BeExam.DAL;
using BeExam.Models;
using BeExam.Utilities.Extentions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BeExam.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class ServiceController : Controller
    {
        private readonly AppDbContext _context;
        private readonly IWebHostEnvironment _env;

        public ServiceController(AppDbContext context,IWebHostEnvironment env)
        {
            _context = context;
            _env = env;
        }

        public async Task<IActionResult> Index()
        {
            List<Service> services = await _context.Services.ToListAsync();
            return View(services);
        }

        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Create(CreateServiceVM serviceVM)
        {
            if(!ModelState.IsValid) return View(serviceVM);

            bool result = await _context.Services.AnyAsync(s=>s.Name== serviceVM.Name);
            if(result==null)
            {
                ModelState.AddModelError("Name", "Bu adda istifadeci movcuddur");
                return View (result);
            }
            if (!serviceVM.Photo.ValidateType())
            {
                ModelState.AddModelError("Photo", "Shekilin tipi uygun deyil");
                return View(serviceVM);
            }
            if (!serviceVM.Photo.ValidateSize(10))
            {
                ModelState.AddModelError("Photo", "Shekilin olcusu uygun deyil");
                return View(serviceVM);
            }
            Service service = new Service
            {
                Name = serviceVM.Name,
                Image = await serviceVM.Photo.CreateFileAsync(_env.WebRootPath, "assets", "images")
            };
            await _context.Services.AddAsync(service);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        public async Task<IActionResult> Update(int id)
        {
            if (id <= 0) return BadRequest();

            Service service = await _context.Services.FirstOrDefaultAsync(s=>s.Id==id); 
            if (service == null) return NotFound();
            UpdateServiceVM update = new UpdateServiceVM
            {
                Name = service.Name,
                Image = service.Image,
            };
            return View(update);
           
        }
        [HttpPost]
        public async Task<IActionResult> Update(int id,UpdateServiceVM serviceVM)
        {
            if(!ModelState.IsValid) return View(serviceVM);

            if (id <= 0) return BadRequest();

            Service service = await _context.Services.FirstOrDefaultAsync(s => s.Id == id);
            if (service == null) return NotFound();

            bool result = await _context.Services.AnyAsync(s => s.Name == serviceVM.Name);
            if (result)
            {
                ModelState.AddModelError("Name", "Bu adda service movcuddur");
                return View(result);
            };

            if(serviceVM.Photo is not null)
            {
                if (!serviceVM.Photo.ValidateType())
                {
                    ModelState.AddModelError("Photo", "Shekilin tipi uygun deyil");
                    return View(serviceVM);
                }
                if (!serviceVM.Photo.ValidateSize(100))
                {
                    ModelState.AddModelError("Photo", "Shekilin olcusu uygun deyil");
                    return View(serviceVM);
                }
                string newImage = await serviceVM.Photo.CreateFileAsync(_env.WebRootPath, "assets", "images");
                service.Image.DeleteFile(_env.WebRootPath, "assets", "images");
                service.Image= newImage;
            }
            service.Name= serviceVM.Name;

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        public async Task<IActionResult> Delete(int id)
        {
            if (id <= 0) return BadRequest();

            Service service = await _context.Services.FirstOrDefaultAsync(s => s.Id == id);
            if (service == null) return NotFound();

            _context.Services.Remove(service);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

      
    }
}
