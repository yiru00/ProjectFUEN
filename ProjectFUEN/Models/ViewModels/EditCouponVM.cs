using Microsoft.CodeAnalysis.CSharp.Syntax;
using ProjectFUEN.Models.DTOs;
using System.ComponentModel.DataAnnotations;

namespace ProjectFUEN.Models.ViewModels
{
    public class EditCouponVM
    {
        public int Id { get; set; } //是不是不用?

        [Display(Name="Code")]
        public string Code { get; set; }

        [Display(Name = "Name")]
        [Required(ErrorMessage = "{0}必填")]
        [StringLength(50)]
        public string Name { get; set; }

        [Display(Name = "Discount")] //不能是負的 小於10000
        [Range(0, 9999,
        ErrorMessage = "Value for {0} must be between {1} and {2}.")]
        [Required(ErrorMessage = "{0}必填")]
        public decimal Discount { get; set; }

        [Display(Name = "LeastCost")]
        [Required(ErrorMessage = "{0}必填")]
        public int LeastCost { get; set; }

        [Display(Name = "Count")]
        [Required(ErrorMessage = "{0}必填")]
        public int Count { get; set; }

        public int discountType { get; set; }
    }

    public static class EditCouponVMExts
    {
        public static EditCouponVM ToEditCouponVM(this CouponDto source)
        {
            return new EditCouponVM()
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
