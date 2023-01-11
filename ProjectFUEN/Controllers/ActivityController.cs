using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ProjectFUEN.Models.EFModels;

namespace ProjectFUEN.Controllers
{
    public class ActivityController : Controller
    {
        private readonly ProjectFUENContext _context;

        public ActivityController(ProjectFUENContext context)
        {
            _context = context;
        }

        // GET: Activity
        public async Task<IActionResult> Index()
        {
            var projectFUENContext = _context.Activities.Include(a => a.Category).Include(a => a.Instructor);
            return View(await projectFUENContext.ToListAsync());
        }

        // GET: Activity/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Activities == null)
            {
                return NotFound();
            }

            var activity = await _context.Activities
                .Include(a => a.Category)
                .Include(a => a.Instructor)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (activity == null)
            {
                return NotFound();
            }

            return View(activity);
        }

        // GET: Activity/Create
        public IActionResult Create()
        {
            ViewData["CategoryId"] = new SelectList(_context.ActivityCategories, "Id", "CategoryName");
            ViewData["InstructorId"] = new SelectList(_context.Instructors, "Id", "Description");
            return View();
        }

        // POST: Activity/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,CoverImage,ActivityName,Recommendation,Address,MemberLimit,NumOfMember,Description,GatheringTime,Deadline,DateOfCreated,InstructorId,CategoryId")] Activity activity)
        {
            if (ModelState.IsValid)
            {
                _context.Add(activity);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["CategoryId"] = new SelectList(_context.ActivityCategories, "Id", "CategoryName", activity.CategoryId);
            ViewData["InstructorId"] = new SelectList(_context.Instructors, "Id", "Description", activity.InstructorId);
            return View(activity);
        }

        // GET: Activity/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Activities == null)
            {
                return NotFound();
            }

            var activity = await _context.Activities.FindAsync(id);
            if (activity == null)
            {
                return NotFound();
            }
            ViewData["CategoryId"] = new SelectList(_context.ActivityCategories, "Id", "CategoryName", activity.CategoryId);
            ViewData["InstructorId"] = new SelectList(_context.Instructors, "Id", "Description", activity.InstructorId);
            return View(activity);
        }

        // POST: Activity/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,CoverImage,ActivityName,Recommendation,Address,MemberLimit,NumOfMember,Description,GatheringTime,Deadline,DateOfCreated,InstructorId,CategoryId")] Activity activity)
        {
            if (id != activity.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(activity);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ActivityExists(activity.Id))
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
            ViewData["CategoryId"] = new SelectList(_context.ActivityCategories, "Id", "CategoryName", activity.CategoryId);
            ViewData["InstructorId"] = new SelectList(_context.Instructors, "Id", "Description", activity.InstructorId);
            return View(activity);
        }

        // GET: Activity/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Activities == null)
            {
                return NotFound();
            }

            var activity = await _context.Activities
                .Include(a => a.Category)
                .Include(a => a.Instructor)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (activity == null)
            {
                return NotFound();
            }

            return View(activity);
        }

        // POST: Activity/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Activities == null)
            {
                return Problem("Entity set 'ProjectFUENContext.Activities'  is null.");
            }
            var activity = await _context.Activities.FindAsync(id);
            if (activity != null)
            {
                _context.Activities.Remove(activity);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ActivityExists(int id)
        {
          return _context.Activities.Any(e => e.Id == id);
        }
    }
}
