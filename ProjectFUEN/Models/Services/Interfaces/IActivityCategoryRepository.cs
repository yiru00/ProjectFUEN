using ProjectFUEN.Models.DTOs;

namespace ProjectFUEN.Models.Services.Interfaces
{
    public interface IActivityCategoryRepository
    {
        IEnumerable<ActivityCategoryDto> Search();
		void Update(ActivityCategoryDto activityCategoryDto);
	}
}
