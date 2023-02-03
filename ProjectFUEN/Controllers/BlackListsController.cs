using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ProjectFUEN.Models.EFModels;
using ProjectFUEN.Models.Infrastructures.ExtensionMethods;
using ProjectFUEN.Models.ViewModels;

namespace ProjectFUEN.Controllers
{
	[Authorize]
    public class BlackListsController : Controller
    {
        private readonly ProjectFUENContext _context;

        public BlackListsController(ProjectFUENContext context)
        {
            _context = context;
        }

        // GET: BlackLists
        public async Task<IActionResult> Index()
        {
			return View(await _context.Members.Where(x => x.IsInBlackList).Select(x => x.EntityToBlackVM()).ToListAsync());
		}

		public IEnumerable<BlackListVM> GetBackListMembers(string account)
		{
			var members = _context.Members
				.Where(x => x.EmailAccount != null && x.IsInBlackList && x.EmailAccount.Contains(account));

			return members.Select(x => x.EntityToBlackVM());
		}

		[HttpPut]
		public void Delete(int id)
		{
			var member = _context.Members.FirstOrDefault(x => x.Id == id);

			member.IsInBlackList = false;

			_context.SaveChanges();
		}
	}
}
