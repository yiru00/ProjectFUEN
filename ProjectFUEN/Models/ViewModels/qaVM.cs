using ProjectFUEN.Models.EFModels;

namespace ProjectFUEN.Models.ViewModels
{
    public class qaVM
    {
        public qaVM()
        {
            Answers = new HashSet<Answer>();
        }

        public int QuestionId { get; set; }
        public string QuestionContent { get; set; }
        public DateTime QuestionDateCreated { get; set; }
        public int ActivityId { get; set; }
        public int MemberId { get; set; }
        public bool? State { get; set; }
        public string AnswerContent { get; set; }
        public DateTime AnswerDateCreated { get; set; }
        public virtual Activity Activity { get; set; }
        public virtual Member Member { get; set; }
        public virtual ICollection<Answer> Answers { get; set; }
    }
	
}
