using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProjectFUEN.Models.EFModels;
using ProjectFUEN.Models.Infrastructures.Repositories;
using ProjectFUEN.Models.Services.Interfaces;
using ProjectFUEN.Models.Services;
using fileUpload.Models;
using Microsoft.AspNetCore.Identity;
using ProjectFUEN.Models;
using ProjectFUEN.Models.ViewModels;

namespace ProjectFUEN.Controllers
{
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
			return View(await _context.Instructors.ToListAsync());
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

			return View(instructor);
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
		public async Task<IActionResult> Create(IFormFile file, [Bind("Id,InstructorName,Description")] InstructorVM instructor)
		{
			//上傳照片
			(bool, string, string) uploadSuccess = fileManager.UploadFile(file);
			if (!uploadSuccess.Item1)
			{
				ViewBag.photo = uploadSuccess.Item2;
				return View(instructor);
			};


			if (ModelState.IsValid)
			{
				_context.Add(instructor.ToEntity(uploadSuccess.Item3));
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
			return View(instructor);
		}

		// POST: Instructor/Edit/5
		// To protect from overposting attacks, enable the specific properties you want to bind to.
		// For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Edit(int id, [Bind("Id,InstructorName,ResumePhoto,Description")] Instructor instructor)
		{
			if (id != instructor.Id)
			{
				return NotFound();
			}

			if (ModelState.IsValid)
			{
				try
				{
					_context.Update(instructor);
					await _context.SaveChangesAsync();
				}
				catch (DbUpdateConcurrencyException)
				{
					if (!InstructorExists(instructor.Id))
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
			return View(instructor);
		}

		// GET: Instructor/Delete/5
		public async Task<IActionResult> Delete(int? id)
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

			return View(instructor);
		}

		// POST: Instructor/Delete/5
		[HttpPost, ActionName("Delete")]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> DeleteConfirmed(int id)
		{
			if (_context.Instructors == null)
			{
				return Problem("Entity set 'ProjectFUENContext.Instructors'  is null.");
			}
			var instructor = await _context.Instructors.FindAsync(id);
			if (instructor != null)
			{
				_context.Instructors.Remove(instructor);
			}

			await _context.SaveChangesAsync();
			return RedirectToAction(nameof(Index));
		}

		private bool InstructorExists(int id)
		{
			return _context.Instructors.Any(e => e.Id == id);
		}
	}
}
