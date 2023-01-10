using ProjectFUEN.Models.DTOs;
using ProjectFUEN.Models.EFModels;
using ProjectFUEN.Models.Services.Interfaces;
using ProjectFUEN.Models.ViewModels;

namespace ProjectFUEN.Models.Infrastructures.Repositories
{
    public class CouponRepository : ICouponRepository
    {
        private ProjectFUENContext db;
        public CouponRepository(ProjectFUENContext db)
        {
            this.db = db;
        }
        public IEnumerable<CouponDto> GetAll()
        {
            IEnumerable<Coupon> coupon = db.Coupons;
            IEnumerable<CouponDto> couponDto = coupon.Select(c => c.ToCouponDto());
            return couponDto;
        }

        public void Create (CouponDto dto)
        {

            db.Add(dto.ToCoupon());
            db.SaveChangesAsync();
        }
    }
}
