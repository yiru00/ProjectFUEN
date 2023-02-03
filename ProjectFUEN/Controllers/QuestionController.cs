using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ProjectFUEN.Models.EFModels;
using ProjectFUEN.Models.ViewModels;

namespace ProjectFUEN.Controllers
{
	[Authorize]
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
			var data = _context.Questions.Include(q => q.Activity).Include(q => q.Member).Include(q => q.Answers).OrderBy(q => q.State).Select(x => x.QToqaVM()).ToListAsync();
			return View(await data);
		}


		// GET: Question/GetQ
		[HttpGet]
		public QaVM GetQ(int qid)
		{
			
			var question = _context.Questions.Include(q => q.Activity).Include(q => q.Member).Include(q => q.Answers).FirstOrDefault(x => x.Id == qid).QToqaVM();

			return question;
		}

		// POST: Question/Create
		[HttpPost]
		public bool Create(int QuestionId, string AnswerContent)
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

		[HttpPut]
		public bool Edit(int AnswerId, string AnswerContent)
		{
			var ans = _context.Answers.Where(x => x.Id == AnswerId).Single();
			ans.Content = AnswerContent;
			ans.DateCreated = DateTime.Now;
			_context.SaveChanges();

			return true;

		}

	}
}
