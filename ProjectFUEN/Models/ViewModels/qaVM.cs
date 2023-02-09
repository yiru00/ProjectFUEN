using ProjectFUEN.Models.EFModels;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;

namespace ProjectFUEN.Models.ViewModels
{
    public class QaVM
    {
        
        public int QuestionId { get; set; }
		[Display(Name = "提問內容")]
		public string QuestionContent { get; set; }
		[Display(Name = "提問日期")]
		public DateTime QuestionDateCreated { get; set; }
        public int ActivityId { get; set; }
		[Display(Name = "活動名稱")]
		public string ActivityName { get; set; }
		[Display(Name = "會員帳號")]
		public string EmailAccount { get; set; }
		public int MemberId { get; set; }
		[Display(Name = "答覆狀態")]
		public bool? State { get; set; }
		public int AnswerId { get; set; }
		public string AnswerContent { get; set; }
        [Display(Name = "答覆日期")]
        public DateTime AnswerDateCreated { get; set; }
	}
	
}
