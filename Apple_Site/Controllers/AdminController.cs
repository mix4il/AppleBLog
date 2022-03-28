using Apple_Site.Data;
using Apple_Site.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Apple_Site.Controllers
{
    public class AdminController : Controller
    {
        ApplicationDbContext db;
        private readonly IWebHostEnvironment _hostEnvironment;
        public AdminController(ApplicationDbContext context, IWebHostEnvironment hostEnvironment)
        {
            db = context;
            _hostEnvironment = hostEnvironment;
        }
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Create(Article art)
        {
 
                string wwwRootPath = _hostEnvironment.WebRootPath;
                string fileName = Path.GetFileNameWithoutExtension(art.ImageFile.FileName);
                string extension = Path.GetExtension(art.ImageFile.FileName);
                art.ImageName = fileName = fileName + DateTime.Now.ToString("yymmssfff") + extension;
                string path = Path.Combine(wwwRootPath + "/Image/", fileName);
                using (var fileStream = new FileStream(path, FileMode.Create))
                {
                    await art.ImageFile.CopyToAsync(fileStream);
                }
                //Insert record
                db.Article.Add(art);
                await db.SaveChangesAsync();

                return View();
        }

        [HttpGet]
        public IActionResult Edit()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Edit(Article article)
        {
            db.Article.Add(article);
            db.SaveChanges();
            return RedirectToAction("Edit");
        }
    }
}
