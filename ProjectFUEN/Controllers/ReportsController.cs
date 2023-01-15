using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ProjectFUEN.Models.EFModels;
using ProjectFUEN.Models.Infrastructures.ExtensionMethods;
using ProjectFUEN.Models.ViewModels;

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

        public IEnumerable<PhotoReportIndexVM> GetAllPhotoReports()
        {
			var photoReport = _context.PhotoReports
                .Include(p => p.Photo)
                .Include(p => p.ReporterNavigation)
                .Select(p => p.PhotoEntityToIndexVM());

			return photoReport.ToList();
		}

		public IEnumerable<CommentReportIndexVM> GetAllCommentReports()
		{
            var commentReport = _context.CommentReports
                .Include(c => c.Comment)
                .Include(c => c.ReporterNavigation)
                .Select(c => c.CommentEntityToIndexVM());

			return commentReport.ToList();
		}

		public IEnumerable<PhotoReportIndexVM> GetPhotoReport(string account)
		{
			var photoReport = _context.PhotoReports
				.Include(p => p.Photo)
				.Include(p => p.ReporterNavigation)
				.Where(c => c.ReporterNavigation.EmailAccount.Contains(account))
				.Select(p => p.PhotoEntityToIndexVM());

			return photoReport.ToList();
		}

		public IEnumerable<CommentReportIndexVM> GetCommentReport(string account)
		{
			var commentReport = _context.CommentReports
				.Include(c => c.Comment)
				.Include(c => c.ReporterNavigation)
                .Where(c => c.ReporterNavigation.EmailAccount.Contains(account))
				.Select(c => c.CommentEntityToIndexVM());

			return commentReport.ToList();
		}
	}
}
