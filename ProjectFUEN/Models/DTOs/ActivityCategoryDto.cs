using ProjectFUEN.Models.EFModels;
using ProjectFUEN.Models.ViewModels;
using System.ComponentModel.DataAnnotations;

namespace ProjectFUEN.Models.DTOs
{
	public class ActivityCategoryDto
	{
		public int Id { get; set; }
		
		public int DisplayOrder { get; set; }
		public string CategoryName { get; set; }
	}
    public static partial class ActivityCategoryExts
    {
        public static ActivityCategoryDto ToDto(this ActivityCategory source)
            => new ActivityCategoryDto
            {
                Id = source.Id,
                DisplayOrder = source.DisplayOrder,
                CategoryName = source.CategoryName,
            };
        public static ActivityCategoryDto ToDto(this ActivityCategoryVM source)
            => new ActivityCategoryDto
            {
                Id = source.Id,
                DisplayOrder = source.DisplayOrder,
                CategoryName = source.CategoryName,
            };

    }
}
