using Microsoft.CodeAnalysis.CSharp.Syntax;
using ProjectFUEN.Models.DTOs;
using System.ComponentModel.DataAnnotations;

namespace ProjectFUEN.Models.ViewModels
{
    public class DeleteCouponVM
    {
        public int Id { get; set; } 

        [Display(Name="Code")]
        public string Code { get; set; }

        [Display(Name = "Name")]
        public string Name { get; set; }

        [Display(Name = "Discount")] 
        public decimal Discount { get; set; }

        [Display(Name = "LeastCost")]
        public int LeastCost { get; set; }

        [Display(Name = "Count")]
        public int Count { get; set; }

        public int discountType { get; set; }
    }

    public static class DeleteCouponVMExts
    {
        public static DeleteCouponVM ToDeleteCouponVM(this CouponDto source)
        {
            return new DeleteCouponVM()
            {
                Id = source.Id,
                Code = source.Code,
                Name = source.Name,
                Discount = source.Discount,
                LeastCost = source.LeastCost,
                Count = source.Count,
                discountType = source.discountType
            };
        }
    }
}
