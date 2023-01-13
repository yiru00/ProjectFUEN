using ProjectFUEN.Models.EFModels;
using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;

namespace ProjectFUEN.Models.ViewModels
{
	public class ActivityVM
	{

		

		public int Id { get; set; }
		
		public string ?CoverImage { get; set; }
		[Display(Name = "活動名稱")]
		public string ActivityName { get; set; }
        [Display(Name = "推薦器材")]
        public string Recommendation { get; set; }
        [Display(Name = "地址")]
        public string Address { get; set; }
        [Display(Name = "參加名額")]
        public int MemberLimit { get; set; }

		public int NumOfMember { get; set; }
        [Display(Name = "活動說明")]
        public string Description { get; set; }
        [Display(Name = "集合時間")]
        public DateTime GatheringTime { get; set; }
        [Display(Name = "截止日")]
        public DateTime Deadline { get; set; }
		public DateTime DateOfCreated { get; set; }
		[Display(Name = "講師")]
		public int? InstructorId { get; set; }

        [Display(Name = "分類")]
        public int? CategoryId { get; set; }

		
	}
    public static partial class ActivityExts
    {
        public static ActivityVM ToVM(this Activity source)
        {
            return new ActivityVM
            {
                Id = source.Id,
                CoverImage= source.CoverImage,
                ActivityName= source.ActivityName,
                Recommendation= source.Recommendation,
                Address= source.Address,
                MemberLimit= source.MemberLimit,
                NumOfMember=0,
                Description=source.Description,
                GatheringTime= source.GatheringTime,
                Deadline= source.Deadline,
                DateOfCreated= source.DateOfCreated,
                InstructorId= source.InstructorId,
                CategoryId = source.CategoryId,

            };
        }


    }
}
