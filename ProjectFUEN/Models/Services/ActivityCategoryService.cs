using Microsoft.EntityFrameworkCore;
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
        {
            //要更新的類別、編號沒有重複(除了自己)=>更新
            if (EditCategoryNameExists(activityCategoryDto))
            {
                throw new Exception("類型已存在");
            }
            else if (EditDisplayOrderExists(activityCategoryDto))
            {
                throw new Exception("編號已存在");
            }
            
            _repository.Update(activityCategoryDto);
        }

        /// <summary>
        /// (編輯用)除了自己以外，是否存在相同的類型名稱
        /// </summary>
        /// <param name="categoryName"></param>
        /// <returns></returns>
        public bool EditCategoryNameExists(ActivityCategoryDto activityCategoryDto)
        {
            return  _repository.FindCategoryNameWithoutSelf(activityCategoryDto) ?true:false;

        }
        /// <summary>
        /// (編輯用)除了自己以外，是否有相同的編號
        /// </summary>
        /// <param name="categoryName"></param>
        /// <returns></returns>
        public bool EditDisplayOrderExists(ActivityCategoryDto activityCategoryDto)
        {
            return _repository.FindDisplayOrderWithoutSelf(activityCategoryDto) ? true : false;

        }
        
        public void Add(ActivityCategoryDto activityCategoryDto)
        {
            //要新增的類別、編號沒有重複=>新增
            if (CreateCategoryNameExists(activityCategoryDto))
            {
                throw new Exception("類型已存在");
            }
            else if (CreateDisplayOrderExists(activityCategoryDto))
            {
                throw new Exception("編號已存在");
            }

            _repository.Add(activityCategoryDto);
        }
        /// <summary>
        /// (新增用)是否存在相同的類型名稱
        /// </summary>
        /// <param name="categoryName"></param>
        /// <returns></returns>
        public bool CreateCategoryNameExists(ActivityCategoryDto activityCategoryDto)
        {
            return _repository.FindCategoryName(activityCategoryDto) ? true : false;

        }
        /// <summary>
        /// (新增用)是否有相同的編號
        /// </summary>
        /// <param name="categoryName"></param>
        /// <returns></returns>
        public bool CreateDisplayOrderExists(ActivityCategoryDto activityCategoryDto)
        {
            return _repository.FindDisplayOrder(activityCategoryDto) ? true : false;

        }
    }
}
