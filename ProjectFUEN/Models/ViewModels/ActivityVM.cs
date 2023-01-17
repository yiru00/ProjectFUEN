using ProjectFUEN.Models.EFModels;
using System.ComponentModel.DataAnnotations;
using System.Runtime.CompilerServices;
using System.Xml.Linq;

namespace ProjectFUEN.Models.ViewModels
{
	public class ActivityVM
	{
		public int Id { get; set; }

        [Display(Name ="活動圖片")]
		public string ?CoverImage { get; set; }

		[Display(Name = "活動名稱")]
        [Required(ErrorMessage ="必填")]
		public string ActivityName { get; set; }
        
        [Display(Name = "推薦器材")]
        [Required(ErrorMessage = "必填")]
        public string Recommendation { get; set; }
        
        [Display(Name = "地點")]
        [Required(ErrorMessage = "必填")]
        public string Address { get; set; }
        
        [Display(Name = "活動名額")]
        [Required(ErrorMessage = "必填")]
        public int MemberLimit { get; set; }
        
        [Display(Name = "報名人數")]
        public int ?NumOfMember { get; set; }
        
        [Display(Name = "活動說明")]
        [Required(ErrorMessage = "必填")]
        public string Description { get; set; }
        
        [Display(Name = "活動集合時間")]
        [Required(ErrorMessage = "必填")]
        public DateTime GatheringTime { get; set; }

        [Display(Name = "報名截止時間")]
        [Required(ErrorMessage = "必填")]
        public DateTime Deadline { get; set; }
        
        [Display(Name = "活動刊登時間")]
        public DateTime DateOfCreated { get; set; }
		
        [Display(Name = "講師")]
        public int? InstructorId { get; set; }

        [Display(Name = "分類")]
        public int? CategoryId { get; set; }
        [Display(Name = "分類")]
        public string ?CategoryName { get; set; }
        [Display(Name = "講師")]

        public string ?InstructorName { get; set; }
    }
    public static partial class ActivityExts
    {
        public static ActivityVM ToVM(this Activity source)
        {
            if (source.CategoryId == null && source.InstructorId == null)
            {
                return new ActivityVM
                {
                    Id = source.Id,
                    CoverImage = source.CoverImage,
                    ActivityName = source.ActivityName,
                    Recommendation = source.Recommendation,
                    Address = source.Address,
                    MemberLimit = source.MemberLimit,
                    NumOfMember = source.ActivityMembers.Count,
                    Description = source.Description,
                    GatheringTime = source.GatheringTime,
                    Deadline = source.Deadline,
                    DateOfCreated = source.DateOfCreated,
                    InstructorId = 0,
                    CategoryId = 0,
                    CategoryName = "未選取分類",
                    InstructorName = "未選取講師"
                };
            }
            else if (source.InstructorId == null)
            {
                return new ActivityVM
                {
                    Id = source.Id,
                    CoverImage = source.CoverImage,
                    ActivityName = source.ActivityName,
                    Recommendation = source.Recommendation,
                    Address = source.Address,
                    MemberLimit = source.MemberLimit,
                    NumOfMember = source.ActivityMembers.Count,
                    Description = source.Description,
                    GatheringTime = source.GatheringTime,
                    Deadline = source.Deadline,
                    DateOfCreated = source.DateOfCreated,
                    InstructorId = 0,
                    CategoryId = source.CategoryId,
                    CategoryName = source.Category.CategoryName,
                    InstructorName = "未選取講師"

                };
            }
            else if(source.CategoryId == null)
			{
				return new ActivityVM
				{
					Id = source.Id,
					CoverImage = source.CoverImage,
					ActivityName = source.ActivityName,
					Recommendation = source.Recommendation,
					Address = source.Address,
					MemberLimit = source.MemberLimit,
					NumOfMember = source.ActivityMembers.Count,
					Description = source.Description,
					GatheringTime = source.GatheringTime,
					Deadline = source.Deadline,
					DateOfCreated = source.DateOfCreated,
					InstructorId = source.InstructorId,
					CategoryId = 0,
					CategoryName = "未選取分類",
					InstructorName = source.Instructor.InstructorName
				};
			}
			
			else {
				return new ActivityVM
				{
					Id = source.Id,
					CoverImage = source.CoverImage,
					ActivityName = source.ActivityName,
					Recommendation = source.Recommendation,
					Address = source.Address,
					MemberLimit = source.MemberLimit,
					NumOfMember = source.ActivityMembers.Count,
					Description = source.Description,
					GatheringTime = source.GatheringTime,
					Deadline = source.Deadline,
					DateOfCreated = source.DateOfCreated,
					InstructorId = source.InstructorId,
					CategoryId = source.CategoryId,
					CategoryName = source.Category.CategoryName,
					InstructorName = source.Instructor.InstructorName

				};
			}
            
        }


    }
}
