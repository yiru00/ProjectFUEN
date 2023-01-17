using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using ProjectFUEN.Models.EFModels;
using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;

namespace ProjectFUEN.Models.ViewModels
{
    public class EventCreateVM
    {
        public int Id { get; set; }


        [Display(Name = "活動名稱")]
        [Required(ErrorMessage = "{0} 必填")]
        public string EventName { get; set; }

        [Display(Name = "活動照片")]
        [Required(ErrorMessage = " {0} 必選")]
        public IFormFile File { get; set; }

        [Required(ErrorMessage = "開始日期必填")]
        [Display(Name = "開始日期")]
        [DataType(DataType.Date)]
        public DateTime StartDate { get; set; }

        [Required(ErrorMessage = "結束日期必填")]
        [Display(Name = "結束日期")]
        [DataType(DataType.Date)]
        public DateTime EndDate { get; set; }
        public int[] CheckBoxes { get; set; }
    }
}
