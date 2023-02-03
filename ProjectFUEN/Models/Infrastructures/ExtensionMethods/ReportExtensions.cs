using ProjectFUEN.Models.EFModels;
using ProjectFUEN.Models.ViewModels;

namespace ProjectFUEN.Models.Infrastructures.ExtensionMethods
{
    public static class ReportExtensions
    {
        public static PhotoReportIndexVM PhotoEntityToIndexVM(this PhotoReport source)
        {
            return new PhotoReportIndexVM()
            {
				Id = source.Id,
				ReporterId = (source.Reporter.HasValue) ? source.Reporter.Value : -1,
                Reporter = source.ReporterNavigation.EmailAccount,
                Photo = source.Photo.Source,
                PhotoId = source.Photo.Id,
				MemberId = source.Photo.Author,
				ReportTime = source.ReportTime.ToString("yyyy-MM-dd")
            };
        }

		public static CommentReportIndexVM CommentEntityToIndexVM(this CommentReport source)
		{
			return new CommentReportIndexVM()
			{
                Id = source.Id,
                ReporterId = (source.Reporter.HasValue) ? source.Reporter.Value : -1,
				Reporter = source.ReporterNavigation.EmailAccount,
				Comment = source.Comment.Content,
                CommentId = source.Comment.Id,
                MemberId = source.Comment.MemberId,
				ReportTime = source.ReportTime.ToString("yyyy-MM-dd")
			};
		}
	}
}
