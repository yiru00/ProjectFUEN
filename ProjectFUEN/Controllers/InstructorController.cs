using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProjectFUEN.Models.EFModels;
using ProjectFUEN.Models.Infrastructures.Repositories;
using ProjectFUEN.Models.Services.Interfaces;
using ProjectFUEN.Models.Services;
using Microsoft.AspNetCore.Identity;
using ProjectFUEN.Models;
using ProjectFUEN.Models.ViewModels;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Authorization;

namespace ProjectFUEN.Controllers
{
	[Authorize]
	public class InstructorController : Controller
	{
		FileManager fileManager;
		private readonly ProjectFUENContext _context;
		private InstructorService instructorService;

		public InstructorController(ProjectFUENContext context)
		{
			fileManager = new FileManager();
			_context = context;
			IInstructorRepository repo = new InstructorRepository(_context);
			this.instructorService = new InstructorService(repo);
		}
		
		// GET: Instructor
		public async Task<IActionResult> Index()
		{
			return View(await _context.Instructors.Select(x=>x.ToVM()).ToListAsync());
		}

		// GET: Instructor/Details/5
		public async Task<IActionResult> Details(int? id)
		{
			if (id == null || _context.Instructors == null)
			{
				return NotFound();
			}

			var instructor = await _context.Instructors
				.FirstOrDefaultAsync(m => m.Id == id);
			if (instructor == null)
			{
				return NotFound();
			}

			return View(instructor.ToVM());
		}

		// GET: Instructor/Create
		public IActionResult Create()
		{
			return View();
		}

		// POST: Instructor/Create
		// To protect from overposting attacks, enable the specific properties you want to bind to.
		// For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Create(IFormFile file,[Bind("Id,InstructorName,Description,ResumePhoto")]InstructorVM instructor)
		{
			
            if (ModelState.IsValid)
			{
				_context.Add(instructor.ToEntity());
				await _context.SaveChangesAsync();
				return RedirectToAction(nameof(Index));

			}

			return View(instructor);
		}

		// GET: Instructor/Edit/5
		public async Task<IActionResult> Edit(int? id)
		{
			if (id == null || _context.Instructors == null)
			{
				return NotFound();
			}

			var instructor = await _context.Instructors.FindAsync(id);
			if (instructor == null)
			{
				return NotFound();
			}
			return View(instructor.ToVM());
		}

		// POST: Instructor/Edit/5
		// To protect from overposting attacks, enable the specific properties you want to bind to.
		// For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Edit(IFormFile file, int id, [Bind("Id,InstructorName,ResumePhoto,Description")] InstructorVM instructor)
		{
			if (id != instructor.Id)
			{
				return NotFound();
			}

            //沒上傳檔案=>用舊的網址（resumePhoto欄位有繫結）
            if (file==null) ModelState.Remove("file"); //不驗證圖片上傳欄位

                if (ModelState.IsValid)
                {
                    _context.Update(instructor.ToEntity());
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));

                }
                return View(instructor);
            
           
        }


        // POST: Instructor/Delete/5
        [HttpDelete]
		public bool Delete(int id)
		{
			if (_context.Instructors == null)
			{
				return false;
			}
			var instructor =  _context.Instructors.Find(id);
			if (instructor != null)
			{
				_context.Instructors.Remove(instructor);
			}

			 _context.SaveChanges();
			return true;
		}

		private bool InstructorExists(int id)
		{
			return _context.Instructors.Any(e => e.Id == id);
		}
	}
}
