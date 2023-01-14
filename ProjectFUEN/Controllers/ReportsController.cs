using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ProjectFUEN.Models.EFModels;
using ProjectFUEN.Models.Infrastructures.ExtensionMethods;

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
            var photoReport = _context.PhotoReports
                .Include(p => p.Photo)
                .Include(p => p.ReporterNavigation)
                .Select(x => x.PhotoEntityToIndexVM());

            return View(await photoReport.ToListAsync());
        }
    }
}
