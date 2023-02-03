using ProjectFUEN.Models.EFModels;
using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;

namespace ProjectFUEN.Models.VM
{
    public class ProductPhotoVM
    {
        public int Id { get; set; }
        [Display(Name = "商品名稱")]
        [Required]
        public int ProductId { get; set; }
        public string? Source { get; set; }
        [Display(Name = "商品圖檔")]
        public IEnumerable<IFormFile> Sources { get; set; }

        //public virtual Product Product { get; set; }
    }
    public static partial class PhotoExts
    {
        public static ProductPhotoVM ToVM(this ProductPhoto source, string sourcephoto)
        {
            return new ProductPhotoVM
            {
                Id = source.Id,
                ProductId = source.ProductId,
                Source = source.Source,
                
            };
        }
    }
    public static partial class PhotoExts
    {
        public static ProductPhoto ToEntity(this ProductPhotoVM source)
        {
            return new ProductPhoto
            {
                Id = source.Id,
                ProductId = source.ProductId,
                Source = source.Source,
            };
        }
    }
}
