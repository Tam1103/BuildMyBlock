using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using MyBlock.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using TechWebsite.Models;

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
