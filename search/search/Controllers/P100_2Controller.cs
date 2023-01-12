using search.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using X.PagedList;
using System.Data.Entity;

namespace search.Controllers
{
    public class P100_2Controller : Controller
    {
        // GET: P100_2
        public ActionResult Index(int PageNumber = 1)
        {
            PageNumber = PageNumber > 0 ? PageNumber : 1;
            IPagedList<Product> pagedData = GetPagedProducts(PageNumber);
            return View(pagedData);
             
        }

        private IPagedList<Product> GetPagedProducts(int pagenumber)
        {
            var db = new AppDbContext();
            int pagesize = 3;
            var query = db.Products.Include(a => a.Category).OrderBy(a => a.Category.DisplayOrder);
            return query.ToPagedList(pagenumber, pagesize);


        }
    }
}