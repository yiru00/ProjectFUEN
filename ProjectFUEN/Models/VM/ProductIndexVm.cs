using ProjectFUEN.Models.EFModels;
using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;

namespace ProjectFUEN.Models.VM
{
    public class ProductIndexVm
    {

        [Display(Name = "分類名稱")]
        public string CategoryName { get; set; }
        [Display(Name = "品牌名稱")]
        public string BrandName { get; set; }
        public int Id { get; set; }
        public int CategoryId { get; set; }
        public int BrandId { get; set; }
        [Display(Name = "商品名稱")]
        public string Name { get; set; }
        [Display(Name = "價格")]
        public int Price { get; set; }
        [Display(Name = "製造日期")]
        [DataType(DataType.Date)]
        public DateTime ManufactorDate { get; set; }
        [DataType(DataType.Date)]
        [Display(Name = "上架日期")]
        public DateTime ReleaseDate { get; set; }
        [Display(Name = "庫存量")]
        public int Inventory { get; set; }
        [Display(Name = "產品明細")]
        public string ProductSpec { get; set; }
        public virtual Product Product { get; set; }
        public virtual Brand Brand { get; set; }
        public string Source { get; set; }


        public virtual Category Category { get; set; }
        public virtual ProductPhoto ProductPhoto { get; set; }
        public virtual ICollection<OrderItem> OrderItems { get; set; }
        public virtual ICollection<ProductPhoto> ProductPhotos { get; set; }
        public virtual ICollection<ShoppingCart> ShoppingCarts { get; set; }

        public virtual ICollection<Event> Events { get; set; }
        public virtual ICollection<Member> Members { get; set; }


       
    }
}
