using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ProjectFUEN.Models.EFModels;
using ProjectFUEN.Models.ViewModels;

namespace ProjectFUEN.Controllers
{
    public class QuestionController : Controller
    {
        private readonly ProjectFUENContext _context;

        public QuestionController(ProjectFUENContext context)
        {
            _context = context;
        }

        // GET: Question
        public async Task<IActionResult> Index()
        {
            var projectFUENContext = _context.Questions.Include(q => q.Activity).Include(q => q.Member).Select(x=>x.ToqaVM()).ToListAsync();
            return View(await projectFUENContext);
        }
        // GET: Answer/Create
        //public IActionResult Create()
        //{
        //    ViewData["QuestionId"] = new SelectList(_context.Questions, "Id", "Content");
        //    return View();
        //}

        // POST: Answer/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> Create([Bind("Id,Content,DateCreated,QuestionId")] Answer answer)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        _context.Add(answer);
        //        await _context.SaveChangesAsync();
        //        return RedirectToAction(nameof(Index));
        //    }
        //    ViewData["QuestionId"] = new SelectList(_context.Questions, "Id", "Content", answer.QuestionId);
        //    return View(answer);
        //}

        // GET: Answer/Create
        public IActionResult Create()
        {
            ViewData["QuestionId"] = new SelectList(_context.Questions, "Id", "Content");
            return View();
        }

        // POST: Answer/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Content,DateCreated,QuestionId")] qaVM answer)
        {
            if (ModelState.IsValid)
            {
                _context.Add(answer);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["QuestionId"] = new SelectList(_context.Questions, "Id", "Content", answer.QuestionId);
            return View(answer);
        }
        // GET: Question/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Questions == null)
            {
                return NotFound();
            }

            var question = await _context.Questions
                .Include(q => q.Activity)
                .Include(q => q.Member)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (question == null)
            {
                return NotFound();
            }

            return View(question);
        }

        // GET: Question/Create
       

        

        

        private bool QuestionExists(int id)
        {
          return _context.Questions.Any(e => e.Id == id);
        }
    }
}
