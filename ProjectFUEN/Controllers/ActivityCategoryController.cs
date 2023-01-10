using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ProjectFUEN.Models.DTOs;
using ProjectFUEN.Models.EFModels;
using ProjectFUEN.Models.Infrastructures.Repositories;
using ProjectFUEN.Models.Services;
using ProjectFUEN.Models.Services.Interfaces;
using ProjectFUEN.Models.ViewModels;

namespace ProjectFUEN.Controllers
{
    public class ActivityCategoryController : Controller
    {
        private readonly ProjectFUENContext _context;
        private ActivityCategoryService activityCategoryService;

        public ActivityCategoryController(ProjectFUENContext context)
        {
            _context = context;
            IActivityCategoryRepository repo = new ActivityCategoryRepository(_context);
            this.activityCategoryService = new ActivityCategoryService(repo);
        }

        // GET: ActivityCategory
        [HttpGet]
        public  ActionResult Index()
        {
            var data = activityCategoryService.GetAll().Select(c => c.ToVM());
              return View(data);
        }
        
        

        // GET: ActivityCategory/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: ActivityCategory/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create([Bind("Id,DisplayOrder,CategoryName")] ActivityCategoryVM activityCategory)
        {

            try
            {
                activityCategoryService.Add(activityCategory.ToDto());
            }
            catch (Exception ex)
            {
                if (ex.Message == "編號已存在")
                {
                    ModelState.AddModelError("DisplayOrder", ex.Message);
                    
                }
                else
                {
                    ModelState.AddModelError("CategoryName", ex.Message);
                }

            }
            if (ModelState.IsValid)
            {
                
                return RedirectToAction(nameof(Index));
            }
            return View(activityCategory);
        }

        // GET: ActivityCategory/Edit/5
        public ActionResult Edit(int? DisplayOrder)
        {
            if (DisplayOrder == null || _context.ActivityCategories == null)
            {
                ViewBag.url = "./Index";
                ViewBag.message = "記得選取欲編輯的拍攝類別";
                return View("../ErrorPage/page404");
            }
            
            var data = _context.ActivityCategories.FirstOrDefault(x=>x.DisplayOrder==DisplayOrder);
            if (data == null)
            {
                ViewBag.url = "./Index";
                ViewBag.message = "找不到欲編輯的拍攝類別";
                return View("../ErrorPage/page404");   
			}
            return View(data.ToVM());
        }

        // POST: ActivityCategory/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public  ActionResult Edit(int DisplayOrder, [Bind("Id,DisplayOrder,CategoryName")] ActivityCategoryVM activityCategory)
        {
            if (DisplayOrder != activityCategory.DisplayOrder)
            {
                return NotFound();
            }
            
            try
            {
                activityCategoryService.Update(activityCategory.ToDto());
            }
            catch (Exception ex)
            {
                if (ex.Message == "編號已存在")
                {
                    ModelState.AddModelError("DisplayOrder", ex.Message);

                }
                else
                {
                    ModelState.AddModelError("CategoryName", ex.Message);
                }

            }

            if (ModelState.IsValid)
            {
                return RedirectToAction("Index");
            }
            else
            {
                return View(activityCategory);
            }

        }

        // GET: ActivityCategory/Delete/5
        public async Task<IActionResult> Delete(int? DisplayOrder)
        {
            if (DisplayOrder == null || _context.ActivityCategories == null)
            {
                ViewBag.url = "./Index";
                ViewBag.message = "記得選取欲刪除的拍攝類別";
                return View("../ErrorPage/page404");
            }
            
			var activityCategory = await _context.ActivityCategories
                .FirstOrDefaultAsync(m => m.DisplayOrder == DisplayOrder);
            if (activityCategory == null)
            {
                ViewBag.url = "./Index";
                ViewBag.message = "找不到欲刪除的拍攝類別";
                return View("../ErrorPage/page404");
            }

            return View(activityCategory.ToDto().ToVM());
        }

        // POST: ActivityCategory/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int DisplayOrder)
        {
            if (_context.ActivityCategories == null)
            {
                return Problem("Entity set 'ProjectFUENContext.ActivityCategories'  is null.");
            }
            var activityCategory =  _context.ActivityCategories.FirstOrDefault(x => x.DisplayOrder == DisplayOrder);
            if (activityCategory != null)
            {
                _context.ActivityCategories.Remove(activityCategory);
            }
            
             _context.SaveChanges();
            return RedirectToAction(nameof(Index));
        }

        private bool ActivityCategoryExists(int DisplayOrder)
        {
          return _context.ActivityCategories.Any(e => e.DisplayOrder == DisplayOrder);
        }
    }
}
