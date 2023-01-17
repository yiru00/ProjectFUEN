using ProjectFUEN.Models.EFModels;
using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;

namespace ProjectFUEN.Models.ViewModels
{
    public class EventVM
    {
        public int Id { get; set; }

        [Display(Name = "活動名稱")]
        [Required(ErrorMessage = "{0} 必填")]
        public string? EventName { get; set; }

        [Display(Name = "活動照片")]
        public string? Photo { get; set; }

        [Required(ErrorMessage = "開始日期必填")]
        [Display(Name = "開始日期")]
        [DataType(DataType.Date)]
        public DateTime StartDate { get; set; }

        [Required(ErrorMessage = "結束日期必填")]
        [Display(Name = "結束日期")]
        [DataType(DataType.Date)]
        public DateTime EndDate { get; set; }

        public ICollection<Product> Products { get; set; }

    }

    public static partial class EventExts
    {
        public static EventVM ToVM(this Event source)
        {
            return new EventVM
            {
                Id = source.Id,
                EventName = source.EventName,
                Photo = source.Photo,
                StartDate = source.StartDate,
                EndDate = source.EndDate,
                Products = source.Products
            };
        }
    }
}
