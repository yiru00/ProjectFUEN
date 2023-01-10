
using ProjectFUEN.Models.DTOs;
using ProjectFUEN.Models.Services.Interfaces;

namespace ProjectFUEN.Models.Services
{
    public class CouponService
    {
        private ICouponRepository repo;
        public CouponService(ICouponRepository repo) 
        {
            this.repo = repo;
        }

        public (bool IsSuccess,string ErrorMsg) Create(CouponDto dto)
        {
            decimal discount = dto.Discount;
            bool discountIsInt = int.TryParse(discount.ToString(), out int result);

            // A. x要是整數且>= 1  B. 1 > x不是整數 >= 0.01   
            // False 不是整數且大於1  不是整數但<=0.01

            if(discount >= 1 && !discountIsInt) return (false, "Discount折價必須是大於1的整數");
            else if(discount <= 0.01m && discountIsInt) return (false, "Discount打折必須是小數點後二位的數字");

            repo.Create(dto);
            return (true, null);
        }
    }
}
