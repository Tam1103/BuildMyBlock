using Microsoft.AspNetCore.Mvc;
using MyBlock.Models;
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
                return View("Index");
            }
    }
}
