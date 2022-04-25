using Apple_Site.Data;
using Apple_Site.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace Apple_Site.Controllers
{
    public class HomeController : Controller
    {
        ApplicationDbContext db;
        const int pageSize = 4;
        public HomeController(ApplicationDbContext context)
        {
            db = context;
        }

        public ActionResult GetArticle(int? id)
        {
            return View(db.Article.FirstOrDefault(p => p.Id == id));
        }


        public ActionResult Index(int? id)
        {
            int page = id ?? 0;
            if (IsAjaxRequest())
            {
                return PartialView("_GetArticles", GetItemsPage(page));
            }
            return View(GetItemsPage(page));
        }

        public bool IsAjaxRequest()
        {
            if (Request == null)
                throw new ArgumentNullException("Request is null");
            if (Request.Headers != null)
                return Request.Headers["X-Requested-With"] == "XMLHttpRequest";
            return false;
        }

        private List<Article> GetItemsPage(int page = 1)
        {
            var itemsToSkip = page * pageSize;

            return db.Article.OrderBy(t => t.Id).Skip(itemsToSkip).
                Take(pageSize).ToList();
        }

        public ActionResult Filter(DateTime dateBefore, DateTime dateAfter, string tag)
        {
            var filterList = db.Article.OrderBy(t => t.Id).Where(p => p.CreateTime > dateBefore)
                .Where(p => p.CreateTime <= dateAfter).Where(p => p.Tag == tag).ToList();
            
                return View(filterList);
        }

        public ActionResult Search(string term)
        {
            var searchList = db.Article.OrderBy(t => t.Id).Where(p => p.Title.Contains(term)).ToList();
            return View(searchList);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
