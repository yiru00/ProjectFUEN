using ProjectFUEN.Models.EFModels;
using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;

namespace ProjectFUEN.Models.DTOs
{
    public class OrderItemsDTO
    {
        public int OrderId { get; set; }
        public int ProductId { get; set; }
        public string ProductName { get; set; }
        public int ProductPrice { get; set; }
        public int ProductNumber { get; set; }
        public virtual OrderDetail Order { get; set; }
        public virtual Product Product { get; set; }
    }
}
