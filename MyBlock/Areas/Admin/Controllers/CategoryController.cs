using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MyBlock.Models;

namespace MyBlock.Areas.Admin.Controllers
{
    [Authorize(Roles = "Admin")]
    [Area("admin")]
    [Route("admin/category")]
    public class CategoryController : Controller
    {
        private DatabaseContext db = new DatabaseContext();
        public CategoryController(DatabaseContext _db)
        {
            db = _db;
        }

        [Route("")]
        [Route("index")]
        public IActionResult Index()
        {
            ViewBag.categories = db.Categories.Where(c => c.Parent == null).ToList();
            return View();
        }

        [Route("add")]
        [HttpGet]
        public IActionResult Add()
        {
            var category = new Category();
            return View("Add", category);
        }


        [Route("add")]
        [HttpPost]
        public IActionResult Add(Category category)
        {
            category.Parent = null;
            db.Categories.Add(category);
            db.SaveChanges();
            return RedirectToAction("index", "category", new { area = "admin" });
        }

        [HttpGet]
        [Route("delete/{id}")]
        public IActionResult Delete(int id)
        {
            var categories = db.Categories.Find(id);
            db.Categories.Remove(categories);
            db.SaveChanges();
            return RedirectToAction("index", "category", new { area = "admin" });
        }

        [HttpGet]
        [Route("edit/{id}")]
        public IActionResult Edit(int id)
        {
            var categories = db.Categories.Find(id);
            return View("Edit", categories);
        }

        [HttpPost]
        [Route("edit/{id}")]
        public IActionResult Edit(int id,Category category)
        {
            var currentCategory = db.Categories.Find(id);
            currentCategory.Name = category.Name;
            currentCategory.Status = category.Status;
            db.SaveChanges();
            return RedirectToAction("index", "category", new { area = "admin" });
        }

        [HttpGet]
        [Route("addsubcategory/{id}")]
        public IActionResult AddSubcategory(int id)
        {
            var categories = new Category()
            {
                ParentId = id
            };

            return View("AddSubcategory", categories);
        }


        [HttpPost]
        [Route("addsubcategory/{subcategoryid}")]
        public IActionResult AddSubcategory(Category category)
        {
            db.Categories.Add(category);
            db.SaveChanges();
            return RedirectToAction("index", "category", new { area = "admin" });
        }
    }
}