using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ProjectFUEN.Models;
using ProjectFUEN.Models.EFModels;
using ProjectFUEN.Models.ViewModels;

namespace ProjectFUEN.Controllers
{
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
            ViewData["InstructorId"] = new SelectList(_context.Instructors, "Id", "InstructorName");
            return View();
        }

        // POST: Activity/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(IFormFile file, [Bind("Id,ActivityName,Recommendation,Address,MemberLimit,NumOfMember,Description,GatheringTime,Deadline,DateOfCreated,InstructorId,CategoryId")] ActivityVM activity)
        {
            //上傳照片
            (bool, string, string) uploadSuccess = fileManager.UploadFile(file);
            if (!uploadSuccess.Item1)
            {
                ViewBag.photoError = uploadSuccess.Item2;
                return View(activity);
            }
            else
            {
                activity.CoverImage = uploadSuccess.Item3;

            }
            if (ModelState.IsValid)
            {
                _context.Add(activity.ToEntity());
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["CategoryId"] = new SelectList(_context.ActivityCategories, "Id", "CategoryName", activity.CategoryId);
            ViewData["InstructorId"] = new SelectList(_context.Instructors, "Id", "InstructorName", activity.InstructorId);
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
            //判斷是否有上傳圖檔，若檔案類型/未上傳 回傳錯誤訊息，上傳成功回傳新檔名，錯誤訊息=""
            (bool, string, string) uploadSuccess = fileManager.UploadFile(file);


            //上傳檔案失敗(沒上傳東西/上傳圖檔以外的)
            if (!uploadSuccess.Item1)//上傳失敗 item1=false
            {

                if (uploadSuccess.Item2 == "記得選取檔案") //未上傳任何檔案，用原有的
                {
                    ModelState.Remove("file");
                    if (ModelState.IsValid)
                    {
                        _context.Update(activity.ToEntity());
                        await _context.SaveChangesAsync();
                        return RedirectToAction(nameof(Index));

                    }
                }
                else if (uploadSuccess.Item2 == "檔案必須是圖片檔案")//上傳成圖檔以外的
                {
                    ViewBag.photoError = uploadSuccess.Item2; //錯誤訊息
                    ViewData["CategoryId"] = new SelectList(_context.ActivityCategories, "Id", "CategoryName", activity.CategoryId);
                    ViewData["InstructorId"] = new SelectList(_context.Instructors, "Id", "InstructorName", activity.InstructorId);
                    return View(activity);
                }
                ViewData["CategoryId"] = new SelectList(_context.ActivityCategories, "Id", "CategoryName", activity.CategoryId);
                ViewData["InstructorId"] = new SelectList(_context.Instructors, "Id", "InstructorName", activity.InstructorId);
                return View(activity);
            }
            else //有上傳檔案=>判斷有沒有跳檔案錯誤的訊息，沒跳就將新的檔案(uploadSuccess.Item3)更新到instructor.ResumePhoto
            {
                if (uploadSuccess.Item2 == "") //上傳圖檔，錯誤訊息=""
                {
                    activity.CoverImage = uploadSuccess.Item3; //傳入新檔名
                    _context.Update(activity.ToEntity());
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
                else //上傳圖檔以外的(ppt.pdf...)
                {
                    ViewBag.photoError = uploadSuccess.Item2;
                    ViewData["CategoryId"] = new SelectList(_context.ActivityCategories, "Id", "CategoryName", activity.CategoryId);
                    ViewData["InstructorId"] = new SelectList(_context.Instructors, "Id", "InstructorName", activity.InstructorId);
                    return View(activity);
                }

            }
            //ViewData["CategoryId"] = new SelectList(_context.ActivityCategories, "Id", "CategoryName", activity.CategoryId);
            //ViewData["InstructorId"] = new SelectList(_context.Instructors, "Id", "InstructorName", activity.InstructorId);
            //return View(activity);
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
