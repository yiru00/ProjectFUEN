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
        public async Task<IActionResult> Create([Bind("Id,DisplayOrder,CategoryName")] ActivityCategory activityCategory)
        {
            if (ModelState.IsValid)
            {
                _context.Add(activityCategory);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(activityCategory);
        }

        // GET: ActivityCategory/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null || _context.ActivityCategories == null)
            {
                return NotFound();
            }

            var data = _context.ActivityCategories.Find(id);
            if (data == null)
            {
                return NotFound();
            }
            return View(data);
        }

        // POST: ActivityCategory/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public  ActionResult Edit(int id, [Bind("Id,DisplayOrder,CategoryName")] ActivityCategory activityCategory)
        {
            if (id != activityCategory.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    activityCategoryService.Update(activityCategory.ToDto());
                    
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ActivityCategoryExists(activityCategory.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(activityCategory);
        }

        // GET: ActivityCategory/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.ActivityCategories == null)
            {
                return NotFound();
            }

            var activityCategory = await _context.ActivityCategories
                .FirstOrDefaultAsync(m => m.Id == id);
            if (activityCategory == null)
            {
                return NotFound();
            }

            return View(activityCategory);
        }

        // POST: ActivityCategory/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.ActivityCategories == null)
            {
                return Problem("Entity set 'ProjectFUENContext.ActivityCategories'  is null.");
            }
            var activityCategory = await _context.ActivityCategories.FindAsync(id);
            if (activityCategory != null)
            {
                _context.ActivityCategories.Remove(activityCategory);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ActivityCategoryExists(int id)
        {
          return _context.ActivityCategories.Any(e => e.Id == id);
        }
    }
}
