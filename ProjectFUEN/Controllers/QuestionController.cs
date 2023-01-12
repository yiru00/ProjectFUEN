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
            var data = _context.Questions.Include(q => q.Activity).Include(q => q.Member).Include(q=>q.Answers).OrderBy(q=>q.State).Select(x=>x.QToqaVM()).ToListAsync();
            return View(await data);
        }
       

        // GET: Question/GetQ
        [HttpGet]
		public QaVM GetQ(int id)
        {
            //不用include answer 會沒東西
			var question = _context.Questions.Include(q => q.Activity).Include(q => q.Member).Include(q => q.Answers).FirstOrDefault(x=>x.Id==id).QToqaVM();
            
			return question;
        }

		// POST: Question/Create
		// To protect from overposting attacks, enable the specific properties you want to bind to.
		// For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
		[HttpPost]
        public bool Create(int QuestionId,string AnswerContent)
        {
            var answer = new Answer()
            {
                QuestionId = QuestionId,
                Content = AnswerContent
            };
			_context.Add(answer);
			_context.SaveChanges();

			return true;
            
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
