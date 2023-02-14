using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ProjectFUEN.Models;
using ProjectFUEN.Models.EFModels;
using ProjectFUEN.Models.ViewModels;

namespace ProjectFUEN.Controllers
{
	[Authorize]
	public class ActivityController : Controller
    {
        FileManager fileManager;
        private readonly ProjectFUENContext _context;

        public ActivityController(ProjectFUENContext context)
        {
            fileManager = new FileManager();
            _context = context;
        }

        // GET: Activity
        public async Task<IActionResult> Index()
        {
            var projectFUENContext = _context.Activities.Include(a => a.Category).Include(a => a.Instructor).Include(a => a.ActivityMembers).Select(a => a.ToVM());

            return View(await projectFUENContext.ToListAsync());
        }

        //get Activity/MemberDetails/5
        [HttpGet]
        public List<ActivityMemberVM> MemberDetails(int id)
        {
            //按照參加時間排
            var activity = _context.ActivityMembers.Include(a => a.Activity).Include(a => a.Member).Where(m => m.ActivityId == id).OrderBy(x => x.DateJoined).Select(m => m.ToActivityMemberVM()).ToList();
            return activity;

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
                .Include(a => a.ActivityMembers)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (activity == null)
            {
                return NotFound();
            }

            return View(activity.ToVM());
        }

        // GET: Activity/Create
        public IActionResult Create()
        {
            ViewData["CategoryId"] = new SelectList(_context.ActivityCategories, "Id", "CategoryName");
            ViewData["InstructorId"] = new SelectList(_context.Instructors, "Id", "InstructorName");
            return View();
        }

        // POST: Activity/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(IFormFile file, [Bind("Id,CoverImage,ActivityName,Recommendation,Address,MemberLimit,Description,GatheringTime,Deadline,DateOfCreated,InstructorId,CategoryId")] ActivityVM activity)
        {
            //if (ModelState.IsValid)
            //{
                _context.Add(activity.ToEntity());
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));

           // }
           // ViewData["CategoryId"] = new SelectList(_context.ActivityCategories, "Id", "CategoryName", activity.CategoryId);
           // ViewData["InstructorId"] = new SelectList(_context.Instructors, "Id", "InstructorName", activity.InstructorId);
           // return View(activity);
           

        }

        // GET: Activity/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Activities == null)
            {
                return NotFound();
            }

            var activity = await _context.Activities
                .Include(a => a.Category)
                .Include(a => a.Instructor)
                .Include(a => a.ActivityMembers)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (activity == null)
            {
                return NotFound();
            }
            ViewData["CategoryId"] = new SelectList(_context.ActivityCategories, "Id", "CategoryName", activity.CategoryId);
            ViewData["InstructorId"] = new SelectList(_context.Instructors, "Id", "InstructorName", activity.InstructorId);
            return View(activity.ToVM());
        }

        // POST: Activity/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(IFormFile file, int id, [Bind("Id,CoverImage,ActivityName,Recommendation,Address,MemberLimit,NumOfMember,Description,GatheringTime,Deadline,DateOfCreated,InstructorId,CategoryId")] ActivityVM activity)
        {
            if (id != activity.Id)
            {
                return NotFound();
            }

            if (activity.MemberLimit < activity.NumOfMember)
            {
                ModelState.AddModelError("MemberLimit", "活動名額不可小於目前報名人數");
            }
            else
            {
                if (file == null) ModelState.Remove("file"); //不驗證圖片上傳欄位

                if (ModelState.IsValid)
                {
                    _context.Update(activity.ToEntity());
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));

                }
                return View(activity);

            }

            ViewData["CategoryId"] = new SelectList(_context.ActivityCategories, "Id", "CategoryName", activity.CategoryId);
            ViewData["InstructorId"] = new SelectList(_context.Instructors, "Id", "InstructorName", activity.InstructorId);
            return View(activity);
        }

        // POST: Activity/Delete/5
        [HttpDelete]
        
        public bool Delete(int id)
        {
            if (_context.Activities == null)
            {
                return false;
            }
            var activity = _context.Activities.Find(id);
            if (activity != null)
            {
                _context.Activities.Remove(activity);
            }

             _context.SaveChanges();
            return true;
        }

        private bool ActivityExists(int id)
        {
            return _context.Activities.Any(e => e.Id == id);
        }
    }
}
