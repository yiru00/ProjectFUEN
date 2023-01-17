using ProjectFUEN.Models.EFModels;
using System.ComponentModel.DataAnnotations;

namespace ProjectFUEN.Models.VM
{
    public class ProductEditVm
    {
        [Display(Name = "商品分類")]
        public int CategoryId { get; set; }
        [Display(Name = "商品品牌")]
        public int BrandId { get; set; }
        public int Id { get; set; }
        [Display(Name = "商品名稱")]
        public string Name { get; set; }
        [Display(Name = "價格")]
        public int Price { get; set; }
        [Display(Name = "製造日期")]
        public DateTime ManufactorDate { get; set; }
        [Display(Name = "庫存量")]
        public int Inventory { get; set; }
        [Display(Name = "產品明細")]
        public string ProductSpec { get; set; }
   
    }
}
