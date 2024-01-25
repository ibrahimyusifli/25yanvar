using BeExam.DAL;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace BeExam.Controllers
{
    public class HomeController : Controller
    {
        private readonly AppDbContext _context;

        public HomeController(AppDbContext context)
        {
            _context = context;
        }
       
        public IActionResult Index()
        {
            return View();
        }

       
    }
}