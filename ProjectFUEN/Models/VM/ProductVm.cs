using ProjectFUEN.Models.EFModels;
using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;

namespace ProjectFUEN.Models.VM
{
    public class ProductVm
    {
        [Display(Name = "商品分類")]
        public int CategoryId { get; set; }
        [Display(Name = "商品品牌")]
        public int BrandId { get; set; }

        //public string CategoryName { get; set; }
        //public string BrandName { get; set; }
        public int Id { get; set; }
        [Display(Name = "商品名稱")]
        public string Name { get; set; }
        [Display(Name = "價格")]
        public int Price { get; set; }
        [Display(Name = "製造日期")]
        [DataType(DataType.Date)]
        public DateTime ManufactorDate { get; set; }
        [Display(Name = "庫存量")]
        public int Inventory { get; set; }
        [Display(Name = "產品明細")]
        public string ProductSpec { get; set; }
        [Display(Name = "產品圖")]
        public IEnumerable<IFormFile> Sources { get; set; }

        // public IEnumerable<String> FileNames { get; set; }

        // public IEnumerable<String> FileNames { get; set; }
    }
}
