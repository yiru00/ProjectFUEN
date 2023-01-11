using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using ProjectFUEN.Models.DTOs;
using ProjectFUEN.Models.EFModels;
using System.ComponentModel.DataAnnotations;

namespace ProjectFUEN.Models.ViewModels
{
	
	public class InstructorVM
    {
        public int Id { get; set; }
        [Display(Name = "姓名")]
        [Required]
        [StringLength(50)]
        public string ?InstructorName { get; set; }
		public string ?ResumePhoto { get; set; }
        [Required]
        [StringLength(300)]
        public string ?Description { get; set; }

    }
	public static partial class InstructorExts
	{
		public static InstructorVM ToVM(this Instructor source)
		{
			return new InstructorVM
			{
				Id = source.Id,
				InstructorName=source.InstructorName,
				ResumePhoto=source.ResumePhoto,
				Description = source.Description

			};
		}
		

	}
}
