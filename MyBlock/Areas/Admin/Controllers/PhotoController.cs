using System;
using System.IO;
using System.Linq;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MyBlock.Models;

namespace MyBlock.Areas.Admin.Controllers
{
    [Authorize(Roles = "Admin")]
    [Area("admin")]
    [Route("admin/photo")]
    public class PhotoController : Controller
    {
        private readonly DatabaseContext db;
        private IHostingEnvironment ihostingEnvironment;

        public PhotoController(DatabaseContext _db, IHostingEnvironment _ihostingEnvironment)
        {
            db = _db;
            ihostingEnvironment = _ihostingEnvironment;
        }
        [Route("index/{id}")]
        public IActionResult Index(int id)
        {
            var photos = db.Photos.Where(p => p.ProductId == id).ToList();
            ViewBag.product = id;
            return View("Index", photos);
        }

        [HttpGet]
        [Route("add/{id}")]
        public IActionResult Add(int id)
        {
            var photo = new Photo()
            {
                ProductId = id
            };
         
            return View("Add", photo);
        }

        [HttpPost]
        [Route("add/{productid}")]
        public IActionResult Add(int productid, Photo photo, IFormFile fileUpload)
        {
            var fileName = DateTime.Now.ToString("MMddyyyyhhmmss") + fileUpload.FileName;
            var path = Path.Combine(this.ihostingEnvironment.WebRootPath, "products", fileName);
            var stream = new FileStream(path, FileMode.Create);
            fileUpload.CopyToAsync(stream);
            photo.Name = fileName;
            db.Photos.Add(photo);
            db.SaveChanges();
            return RedirectToAction("index", "photo", new { area = "admin", id = productid });
        }

        [HttpGet]
        [Route("delete/{id}/productId")]
        public IActionResult Delete(int id, int productId)
        {
            var photo = db.Photos.Find(id);
            db.Photos.Remove(photo);
            db.SaveChanges();
            return RedirectToAction("index", "photo", new { area = "admin", id = productId});
        }

        [HttpGet]
        [Route("edit/{id}/productId")]
        public IActionResult Edit(int id)
        {
            var photo = db.Photos.Find(id);
            return View("Edit",photo);
        }


        [HttpPost]
        [Route("edit/{id}/productId")]
        public IActionResult Edit(int id,int productId, Photo photo, IFormFile fileUpload)
        {
            var currentPhoto = db.Photos.Find(id);

            if (fileUpload != null && !string.IsNullOrEmpty(fileUpload.FileName))
            {
                var fileName = DateTime.Now.ToString("MMddyyyyhhmmss") + fileUpload.FileName;
                var path = Path.Combine(this.ihostingEnvironment.WebRootPath, "products", fileName);
                var stream = new FileStream(path, FileMode.Create);
                fileUpload.CopyToAsync(stream);
                currentPhoto.Name = fileName;
            }
            currentPhoto.Status = photo.Status;
            currentPhoto.Featured = photo.Featured;
            db.SaveChanges();
            return RedirectToAction("index", "photo", new { area = "admin", id = productId});
        }
    }
}