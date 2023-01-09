using ProjectFUEN.Models.DTOs;
using ProjectFUEN.Models.EFModels;
using ProjectFUEN.Models.Services.Interfaces;

namespace ProjectFUEN.Models.Services
{
    public class ActivityCategoryService
    {
        private readonly IActivityCategoryRepository _repository;
        public ActivityCategoryService(IActivityCategoryRepository repository)
        {
            _repository = repository;
        }
        public IEnumerable<ActivityCategoryDto> GetAll()
            => _repository.Search();
        public void Update(ActivityCategoryDto activityCategoryDto)
            => _repository.Update(activityCategoryDto);
	}
}
