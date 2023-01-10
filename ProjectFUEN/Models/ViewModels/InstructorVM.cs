using System.ComponentModel.DataAnnotations;

namespace ProjectFUEN.Models.ViewModels
{
    public class InstructorVM
    {
        public int Id { get; set; }
        [Display(Name = "姓名")]
        [Required]
        [StringLength(50)]
        public string InstructorName { get; set; }

        //public string ResumePhoto { get; set; }
        [Required]
        [StringLength(300)]
        public string Description { get; set; }

    }
}
