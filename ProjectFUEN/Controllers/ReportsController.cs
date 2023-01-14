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
    public class ReportsController : Controller
    {
        private readonly ProjectFUENContext _context;

        public ReportsController(ProjectFUENContext context)
        {
            _context = context;
        }

        // GET: Reports
        public async Task<IActionResult> Index()
        {
            var projectFUENContext = _context.PhotoReports.Include(p => p.Photo).Include(p => p.ReporterNavigation);
            return View(await projectFUENContext.ToListAsync());
        }
    }
}
