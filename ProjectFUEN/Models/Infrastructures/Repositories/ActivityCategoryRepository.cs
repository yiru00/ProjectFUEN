using Microsoft.EntityFrameworkCore;
using ProjectFUEN.Models.DTOs;
using ProjectFUEN.Models.EFModels;
using ProjectFUEN.Models.Services.Interfaces;
using ProjectFUEN.Models.ViewModels;
using System;

namespace ProjectFUEN.Models.Infrastructures.Repositories
{
    public class ActivityCategoryRepository:IActivityCategoryRepository
    {
        private readonly ProjectFUENContext _db;

        public ActivityCategoryRepository(ProjectFUENContext db) 
        {
            _db = db;
        }
        public IEnumerable<ActivityCategoryDto> Search()
        {
            return _db.ActivityCategories.OrderBy(x=>x.DisplayOrder).Select(c=>c.ToDto());
        }
        /// <summary>
        /// 在資料表ActivityCategories有找到CategroyName欄位跟輸入值categroyName一樣(除了自己)=>true
        /// </summary>
        /// <param name="categroyName"></param>
        /// <returns></returns>
        public bool FindCategoryNameWithoutSelf(ActivityCategoryDto activityCategoryDto)
        {
            return _db.ActivityCategories.Where(x => x.Id != activityCategoryDto.Id).Any(e => e.CategoryName == activityCategoryDto.CategoryName);
        }
        
        public bool FindDisplayOrderWithoutSelf(ActivityCategoryDto activityCategoryDto)
        {
            return _db.ActivityCategories.Where(x => x.Id != activityCategoryDto.Id).Any(e => e.DisplayOrder == activityCategoryDto.DisplayOrder);
        }

        public void Update(ActivityCategoryDto activityCategoryDto)
        {
            _db.Update(activityCategoryDto.ToEntity());
            _db.SaveChanges();
        }

        public bool FindCategoryName(ActivityCategoryDto activityCategoryDto)
        {
            return _db.ActivityCategories.Any(e => e.CategoryName == activityCategoryDto.CategoryName);
        }

        public bool FindDisplayOrder(ActivityCategoryDto activityCategoryDto)
        {
            return _db.ActivityCategories.Any(e => e.DisplayOrder == activityCategoryDto.DisplayOrder);
        }
        

        public void Add(ActivityCategoryDto activityCategoryDto)
        {
            _db.Add(activityCategoryDto.ToEntity());
            _db.SaveChanges();
        }
    }
}
