using ProjectFUEN.Models.EFModels;
using ProjectFUEN.Models.ViewModels;

namespace ProjectFUEN.Models.DTOs
{
    public class CouponDto
    {
        public int Id { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public decimal Discount { get; set; }
        public int LeastCost { get; set; }
        public int Count { get; set; }

        public int discountType { get; set; }
    }

    public static class CouponExts
    {
        public static CouponDto ToCouponDto(this Coupon source)
        {
            return new CouponDto
            {
                Id = source.Id,
                Code = source.Code,
                Name = source.Name,
                Discount = source.Discount,
                LeastCost= source.LeastCost,
                Count= source.Count
            };
        }

        public static CouponDto CreateVMToDto(this CreateCouponVM source)
        {
            return new CouponDto
            {
                Id = source.Id,
                Code = source.Code,
                Name = source.Name,
                Discount = source.Discount,
                LeastCost = source.LeastCost,
                Count = source.Count,
                discountType= source.discountType,
            };
        }

        public static CouponDto EditVMToDto(this EditCouponVM source)
        {
            return new CouponDto
            {
                Id = source.Id,
                Code = source.Code,
                Name = source.Name,
                Discount = source.Discount,
                LeastCost = source.LeastCost,
                Count = source.Count,
                discountType = source.discountType,
            };
        }

        public static CouponDto DeleteVMToDto(this DeleteCouponVM source)
        {
            return new CouponDto
            {
                Id = source.Id,
                Code = source.Code,
                Name = source.Name,
                Discount = source.Discount,
                LeastCost = source.LeastCost,
                Count = source.Count,
                discountType = source.discountType,
            };
        }
    }
}
