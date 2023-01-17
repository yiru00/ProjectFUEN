using Microsoft.CodeAnalysis.CSharp.Syntax;
using ProjectFUEN.Models.DTOs;
using System.ComponentModel.DataAnnotations;

namespace ProjectFUEN.Models.ViewModels
{
    public class CouponVM
    {
        public int Id { get; set; }

        public string Code { get; set; }

        public string Name { get; set; }

        public decimal Discount { get; set; }

        public int LeastCost { get; set; }

        public int Count { get; set; }
    }

    public static class CouponVMExts
    {
        public static CouponVM ToCouponVM(this CouponDto source)
        {
            return new CouponVM()
            {
                Id = source.Id,
                Code = source.Code,
                Name = source.Name,
                Discount = source.Discount,
                LeastCost = source.LeastCost,
                Count = source.Count
            };
        }
    }
}
