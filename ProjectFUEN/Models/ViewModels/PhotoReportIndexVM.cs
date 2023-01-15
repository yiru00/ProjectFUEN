namespace ProjectFUEN.Models.ViewModels
{
    public class PhotoReportIndexVM
    {
        public int Id { get; set; }
        public int ReporterId { get; set; }
        public string Reporter { get; set; }
        public int PhotoId { get; set; }
        public string Photo { get; set; }
        public int MemberId { get; set; }
        public string ReportTime { get; set; }
    }
}
