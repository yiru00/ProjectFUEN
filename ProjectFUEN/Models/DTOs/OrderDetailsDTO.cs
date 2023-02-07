using ProjectFUEN.Models.EFModels;
using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;
using System;
using System.Collections.Generic;


namespace ProjectFUEN.Models.DTOs
{
    public class OrderDetailsDTO
    {
      
        public int Id { get; set; }
        public int MemberId { get; set; }
        [Display(Name = "訂單日期")]
        public DateTime OrderDate { get; set; }
        [Display(Name = "地址")]
        public string Address { get; set; }
        [Display(Name = "訂單狀態")]
        public int State { get; set; }

        [Display(Name = "會員帳號")]
        public string EmailAccount { get; set; }

        public virtual Member Member { get; set; }
        public virtual ICollection<OrderItemVM> OrderItems { get; set; }

        
    }
}
