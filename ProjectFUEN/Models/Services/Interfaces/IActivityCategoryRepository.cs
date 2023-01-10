using ProjectFUEN.Models.DTOs;

namespace ProjectFUEN.Models.Services.Interfaces
{
    public interface IActivityCategoryRepository
    {
        void Update(ActivityCategoryDto activityCategoryDto);
        bool FindCategoryNameWithoutSelf(ActivityCategoryDto activityCategoryDto);
        bool FindDisplayOrderWithoutSelf(ActivityCategoryDto activityCategoryDto);
      
        IEnumerable<ActivityCategoryDto> Search();
		
        void Add(ActivityCategoryDto activityCategoryDto);
        bool FindCategoryName(ActivityCategoryDto activityCategoryDto);
        bool FindDisplayOrder(ActivityCategoryDto activityCategoryDto);
    }
}
