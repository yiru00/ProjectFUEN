using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net.WebSockets;
using System.Runtime.InteropServices;
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

		public CommentReportIndexVM GetOneCommentReport(int id)
		{
            var commentReport = _context.CommentReports
                .Include(c => c.Comment)
				.Include(c => c.ReporterNavigation)
				.First(c => c.Comment.Id == id)
                .CommentEntityToIndexVM();

			return commentReport;
		}

		[HttpDelete]
        public void DeletePhoto(PhotoReportIndexVM vm)
        {
            // Delete的照片, memberId不可能是
			AddToIndiscriminate(vm.MemberId);

            // delete Photo
            Photo photo = _context.Photos.First(x => x.Id == vm.PhotoId);
            _context.Remove(photo);
            _context.SaveChanges();
        }

		[HttpDelete]
		public void DeleteComment(CommentReportIndexVM vm)
		{
			// Delete的照片, memberId不可能是
			AddToIndiscriminate(vm.MemberId);

            // delete Comment
            Comment comment = _context.Comments.First(x => x.Id == vm.CommentId);
            _context.Remove(comment);
            _context.SaveChanges();
        }

		[HttpDelete]
		public void DeletePhotoReport(string strreports, string strreporters)
		{
			int[] reports = strreports.Split(',').Select(x => int.Parse(x)).ToArray();
			int[] reporters = strreporters.Split(',').Select(x => int.Parse(x)).ToArray();

			// 將這些Reporter加進Indiscriminate
			foreach (var reporterId in reporters)
			{
				AddToIndiscriminate(reporterId);

			}

			// Remove這些Reporter
			List<PhotoReport> commentReports = new List<PhotoReport>();
			foreach (var id in reports)
			{
				commentReports.Add(_context.PhotoReports.First(c => c.Id == id));
			}

			_context.RemoveRange(commentReports);
			_context.SaveChanges();
		}

		[HttpDelete]
		public void DeleteCommentReport(string strreports, string strreporters)
		{
			int[] reports = strreports.Split(',').Select(x => int.Parse(x)).ToArray();
			int[] reporters = strreporters.Split(',').Select(x => int.Parse(x)).ToArray();

			//將這些Reporter加進Indiscriminate

			foreach (var reporterId in reporters)
			{
				AddToIndiscriminate(reporterId);

			}

			// Remove這些Reporter
			List<CommentReport> commentReports = new List<CommentReport>();
			foreach (var id in reports)
			{
				commentReports.Add(_context.CommentReports.First(c => c.Id == id));
			}

			_context.RemoveRange(commentReports);
			_context.SaveChanges();
		}

		private void AddToIndiscriminate(int memberId)
        {
            // 若找不到reporter ReporterId = -1
            if (memberId < 0) return;

            // Member是否已經在Indiscriminate
            bool isInIndiscriminate = _context.IndiscriminateReports.Any(p => p.MemberId == memberId);

            // 將次數增加
            if (isInIndiscriminate)
            {
				IndiscriminateReport report = _context.IndiscriminateReports.First(p => p.MemberId == memberId);
                report.NumOfTimes += 1;
                _context.SaveChanges();

                return;
            }

            // 加入一筆新資料
            IndiscriminateReport reportMember = new IndiscriminateReport()
            {
                MemberId = memberId,
                NumOfTimes = 1,
            };
            _context.IndiscriminateReports.Add(reportMember);
            _context.SaveChanges();
		}
	}
}
