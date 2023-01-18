namespace ProjectFUEN.Models.ViewModels
{
    public class CommentReportIndexVM
    {
		public int Id { get; set; }
		public int ReporterId { get; set; }
		public string Reporter { get; set; }
		public int CommentId { get; set; }
		public string Comment { get; set; }
		public int MemberId { get; set; }
		public string ReportTime { get; set; }
	}
}
