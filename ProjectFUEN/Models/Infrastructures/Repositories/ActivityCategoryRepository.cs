using Microsoft.EntityFrameworkCore;
using ProjectFUEN.Models.DTOs;
using ProjectFUEN.Models.EFModels;
using ProjectFUEN.Models.Services.Interfaces;
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

		public void Update(ActivityCategoryDto activityCategoryDto)
		{

            _db.Update(activityCategoryDto.ToEntity());
            _db.SaveChanges();
		}
	}
}
