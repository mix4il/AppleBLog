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

        public ActionResult ArticleSearch(string name)
        {
            var article = db.Article.Where(a => a.Title == name).FirstOrDefault();
            ViewBag.model = article;
            return PartialView(article);
        }


        [HttpPost]
        public async Task<IActionResult> EditAsync(Article article)
        {
            string wwwRootPath = _hostEnvironment.WebRootPath;
            string fileName = Path.GetFileNameWithoutExtension(article.ImageFile.FileName);
            string extension = Path.GetExtension(article.ImageFile.FileName);
            article.ImageName = fileName = fileName + DateTime.Now.ToString("yymmssfff") + extension;
            string path = Path.Combine(wwwRootPath + "/Image/", fileName);
            using (var fileStream = new FileStream(path, FileMode.Create))
            {
                await article.ImageFile.CopyToAsync(fileStream);
            }
            db.Article.Update(article);
            db.SaveChanges();
            return RedirectToAction("Edit");
        }

        public ActionResult AutocompleteSearch(string term)
        {
            var models = db.Article.Where(a => a.Title.Contains(term))
                            .Select(a => new { value = a.Title })
                            .Distinct();

            return Json(models);
        }

        public ActionResult IndexSearch()
        {
            return View();
        }


    }
}
