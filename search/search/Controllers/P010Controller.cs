using search.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data.Entity;
using Microsoft.Ajax.Utilities;

namespace search.Controllers
{
    public class P010Controller : Controller
    {
        private AppDbContext db = new AppDbContext();
                  
        // GET: P010
        public ActionResult Index(int? categoryId ,string productName)
        {
            ViewBag.categoryId = GetCategories(categoryId);
            ViewBag.productName = productName;
            var data = db.Products.Include(x => x.Category);

            if (categoryId.HasValue) data = data.Where(p => p.CategoryId == categoryId.Value);


            if (string.IsNullOrEmpty(productName) == false) data = data.Where(p => p.Name.Contains(productName));
            return View(data);
        }

        private IEnumerable<SelectListItem>
            GetCategories(int? categoryId)
        {
            var items = db.Categories
                .Select(c=>new SelectListItem { Value = c.Id.ToString(),Text = c.Name ,Selected=(categoryId.HasValue && c.Id == categoryId.Value)}).ToList().Prepend(new SelectListItem {Value= string.Empty,Text = "請選擇..."});
        
            return items;
        }
    }

}