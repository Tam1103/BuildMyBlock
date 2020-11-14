using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MyBlock.Models;

namespace MyBlock.Areas.Admin.Controllers
{
    [Authorize(Roles = "Admin")]
    [Area("admin")]
    [Route("admin/slideshow")]
    public class SlideShowController : Controller
    {
        private readonly DatabaseContext db ;
        private IHostingEnvironment ihostingEnvironment;

        public SlideShowController(DatabaseContext _db, IHostingEnvironment _ihostingEnvironment)
        {
            db = _db;
            ihostingEnvironment = _ihostingEnvironment;
        }
        [Route("")]
        [Route("index")]
        public IActionResult Index()
        {
            var slideShow = db.SlideShows.ToList();
            return View("index",slideShow);
        }

        [HttpGet]
        [Route("add")]
        public IActionResult Add()
        {
            var slideshow = new SlideShow();
            return View("Add",slideshow);
        }

        [HttpPost]
        [Route("add")]
        public IActionResult Add(SlideShow slideshow,IFormFile photo)
        {
            var fileName = DateTime.Now.ToString("MMddyyyyhhmmss") + photo.FileName; 
            var path = Path.Combine(this.ihostingEnvironment.WebRootPath,"slideshows",fileName);
            var stream = new FileStream(path, FileMode.Create);
            photo.CopyToAsync(stream);
            slideshow.Name = fileName;
            db.SlideShows.Add(slideshow);
            db.SaveChanges();
            return RedirectToAction("index", "slideshow", new { area = "admin" });
        }

        [HttpGet]
        [Route("delete/{id}")]
        public IActionResult Delete(int id)
        {
            var slideshow = db.SlideShows.Find(id);
            db.SlideShows.Remove(slideshow);
            db.SaveChanges();
            return RedirectToAction("index", "slideshow", new { area = "admin" });
        }


        [HttpGet]
        [Route("edit/{id}")]
        public IActionResult Edit(int id)
        {
            var slideshow = db.SlideShows.Find(id);
            return View("Edit", slideshow);
        }

        [HttpPost]
        [Route("edit/{id}")]
        public IActionResult Edit(int id, SlideShow slideshow, IFormFile photo)
        {
            var currentSlideShow = db.SlideShows.Find(id);
           
            if (photo !=null && !string.IsNullOrEmpty(photo.FileName))
            {
                var fileName = DateTime.Now.ToString("MMddyyyyhhmmss") + photo.FileName;
                var path = Path.Combine(this.ihostingEnvironment.WebRootPath, "slideshows", fileName);
                var stream = new FileStream(path, FileMode.Create);
                photo.CopyToAsync(stream);
                currentSlideShow.Name = fileName;
            }
            currentSlideShow.Title = slideshow.Title;
            currentSlideShow.UptoSale = slideshow.UptoSale;
            currentSlideShow.Description = slideshow.Description;
            currentSlideShow.Status =  slideshow.Status;
            db.SaveChanges();
            return RedirectToAction("index", "slideshow", new { area = "admin" });
        }
    }
}