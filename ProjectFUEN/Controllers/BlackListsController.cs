using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ProjectFUEN.Models.EFModels;

namespace ProjectFUEN.Controllers
{
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
              return View(await _context.Members.ToListAsync());
        }
    }
}
