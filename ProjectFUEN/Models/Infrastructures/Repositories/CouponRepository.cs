using Humanizer;
using Microsoft.EntityFrameworkCore;
using ProjectFUEN.Models.DTOs;
using ProjectFUEN.Models.EFModels;
using ProjectFUEN.Models.Services.Interfaces;
using ProjectFUEN.Models.ViewModels;
using System.Dynamic;

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
            db.Coupons.Add(dto.ToCoupon());
            db.SaveChangesAsync();
        }

        public (bool isExist, CouponDto dto) CouponIsExist(int? id)
        {
            Coupon data = db.Coupons.Find(id);
            if (data == null) return (false, null);

            return (true, data.ToCouponDto());
        }

        public void Edit(CouponDto dto)
        {
            Coupon data = db.Coupons.Find(dto.Id);
            data.Id = dto.Id;
            
            db.Entry(data).CurrentValues.SetValues(dto.ToCoupon());

            //data.Name = dto.Name;
            //data.Code = dto.Code;
            //data.LeastCost = dto.LeastCost;
            //data.Count = dto.Count;
            //data.Discount = dto.Discount;


            db.SaveChangesAsync();
        }

        public void Delete(int id)
        {
            Coupon data = db.Coupons.Find(id);

            db.Coupons.Remove(data);
            db.SaveChangesAsync();
        }

        public IEnumerable<string> GetAllEmails()
        {
            return db.Members.Select(m => m.EmailAccount).ToList();
        }
    }
}
