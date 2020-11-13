using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using MyBlock.Models;
using X.PagedList;

namespace MyBlock.Controllers
{
    [Route("product")]
    public class ProductController : Controller
    {
        private readonly DatabaseContext db;
        public ProductController(DatabaseContext _db)
        {
            db = _db;
        }
        [Route("")]
        [Route("index")]
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        [Route("productdisplay/{id}")]
        public IActionResult ProductDisplay(int id, int? page)
        {
            var pageNumber = page ?? 1;
            var category = db.Categories.Find(id);
            ViewBag.category = category;
            ViewBag.nameCategory = category.Name;
            var product = category.Products.Where(s => s.Status).ToList().ToPagedList(pageNumber, 1);
            if (!product.Any())
            {
                ViewBag.notification = "Sorry we are updating, Thanks";
            }
            return View("ProductDisplay",product);
        }
        
        [HttpGet]
        [Route("details/{id}")]
        public IActionResult Details(int id)
        {
            var product = db.Products.Find(id);
            ViewBag.featuredPhoto = product.Photos.Where(p => p.Status && p.Featured).ToList();
            var category = db.Categories.Find(product.CategoryId);
            ViewBag.nameCategory = category.Name;

            ViewBag.releatedProducts = db.Products.Where(p => p.CategoryId == product.CategoryId && p.Id != product.Id && p.Status).ToList();
           
            return View("Details", product);
        }
    }
}