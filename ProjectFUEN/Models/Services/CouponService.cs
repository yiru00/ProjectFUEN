
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
    }
}
