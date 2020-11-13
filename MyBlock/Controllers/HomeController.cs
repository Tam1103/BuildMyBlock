using Microsoft.AspNetCore.Mvc;
using MyBlock.Models;
using System.Linq;

namespace MyBlock.Controllers
{
    [Route("home")]
    public class HomeController : Controller
    {
        private readonly DatabaseContext _context;
        public HomeController(DatabaseContext context)
        {
            _context = context;
        }

        [Route("")]
        [Route("index")]
        [Route("~/")]
        public IActionResult Index()
        {
            var product = _context.Products.OrderByDescending(p => p.Id).Where(p => p.Status).Take(4).ToList();
            return View("Index", product);
        }

        [Route("error")]
        public IActionResult Error()
        {
            return View();
        }

    }
}
