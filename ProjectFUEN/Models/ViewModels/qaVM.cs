using ProjectFUEN.Models.EFModels;

namespace ProjectFUEN.Models.ViewModels
{
    public class QaVM
    {
        
        public int QuestionId { get; set; }
        public string QuestionContent { get; set; }
        public DateTime QuestionDateCreated { get; set; }
        public int ActivityId { get; set; }
        public string ActivityName { get; set; }
        public string EmailAccount { get; set; }
		public int MemberId { get; set; }
		public bool? State { get; set; }

		public string AnswerContent { get; set; }
		public DateTime AnswerDateCreated { get; set; }
	}
	
}
