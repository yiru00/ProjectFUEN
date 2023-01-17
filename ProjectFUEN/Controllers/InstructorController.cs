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
		public async Task<IActionResult> Create(IFormFile file, [Bind("Id,InstructorName,Description")]InstructorVM instructor)
		{
			//上傳照片
			(bool, string, string) uploadSuccess = fileManager.UploadFile(file);
			if (!uploadSuccess.Item1)
			{
				ViewBag.photoError = uploadSuccess.Item2;
				return View(instructor);
			}
			else
			{
				instructor.ResumePhoto = uploadSuccess.Item3;

			}

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
			//判斷是否有上傳圖檔，若檔案類型/未上傳 回傳錯誤訊息，上傳成功回傳新檔名，錯誤訊息=""
			(bool, string, string) uploadSuccess = fileManager.UploadFile(file);


			//上傳檔案失敗(沒上傳東西/上傳圖檔以外的)
			if (!uploadSuccess.Item1)//上傳失敗 item1=false
			{
				
				if (uploadSuccess.Item2== "記得選取檔案") //未上傳任何檔案，用原有的
				{
					ModelState.Remove("file");
					if (ModelState.IsValid)
					{
						_context.Update(instructor.ToEntity());
						await _context.SaveChangesAsync();
						return RedirectToAction(nameof(Index));

					}
				}
				else if (uploadSuccess.Item2== "檔案必須是圖片檔案")//上傳成圖檔以外的
				{
					ViewBag.photoError = uploadSuccess.Item2; //錯誤訊息
					return View(instructor);
				}
				return View(instructor);
			}
			else //有上傳檔案=>判斷有沒有跳檔案錯誤的訊息，沒跳就將新的檔案(uploadSuccess.Item3)更新到instructor.ResumePhoto
			{
				if (uploadSuccess.Item2== "") //上傳圖檔，錯誤訊息=""
				{
					instructor.ResumePhoto = uploadSuccess.Item3; //傳入新檔名
					_context.Update(instructor.ToEntity());
					await _context.SaveChangesAsync();
					return RedirectToAction(nameof(Index));
				}
				else //上傳圖檔以外的(ppt.pdf...)
				{
					ViewBag.photoError = uploadSuccess.Item2;
					return View(instructor);
				}
				
			}

			
		}

		// GET: Instructor/Delete/5
		//public async Task<IActionResult> Delete(int? id)
		//{
		//	if (id == null || _context.Instructors == null)
		//	{
		//		return NotFound();
		//	}

		//	var instructor = await _context.Instructors
		//		.FirstOrDefaultAsync(m => m.Id == id);
		//	if (instructor == null)
		//	{
		//		return NotFound();
		//	}

		//	return View(instructor);
		//}

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
