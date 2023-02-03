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
    public class MembersController : Controller
    {
        private readonly ProjectFUENContext _context;

        public MembersController(ProjectFUENContext context)
        {
            _context = context;
        }

        // GET: Members
        public async Task<IActionResult> Index()
        {
              return View(await _context.Members.Select(x => x.EntityoIndexVM()).ToListAsync());
        }

		public IEnumerable<MemberIndexVM> GetMembers(string account, string selectCity)
		{

            IEnumerable<Member> members = _context.Members;

			if (!string.IsNullOrEmpty(account)) members = members.Where(x => x.EmailAccount != null && x.EmailAccount.Contains(account));

			if (selectCity != "請選擇") members = members.Where(x => x.Address != null && x.Address.Contains(selectCity));

            return members.Select(x => x.EntityoIndexVM()).ToList();
		}
	}
}
