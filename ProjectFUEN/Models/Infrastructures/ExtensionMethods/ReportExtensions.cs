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
                Reporter = source.ReporterNavigation.EmailAccount,
                Photo = source.Photo.Source,
                ReportTime = source.ReportTime.ToString("yyyy-MM-dd")
            };
        }

		public static CommentReportIndexVM CommentEntityToIndexVM(this CommentReport source)
		{
			return new CommentReportIndexVM()
			{
				Reporter = source.ReporterNavigation.EmailAccount,
				Comment = source.Comment.Content.Substring(0, 10) + "...",
				ReportTime = source.ReportTime.ToString("yyyy-MM-dd")
			};
		}
	}
}
