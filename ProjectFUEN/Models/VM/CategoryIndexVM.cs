using ProjectFUEN.Models.EFModels;
using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;

namespace ProjectFUEN.Models.VM
{
	public class CategoryIndexVM
	{
      

        public int Id { get; set; }
		[Display(Name = "分類名稱")]
		public string Name { get; set; }

       
    }
}
