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
       
        public DateTime OrderDate { get; set; }
 
        public string Address { get; set; }
        
        public int State { get; set; }

        //public string EmailAccount { get; set; }

        public virtual Member Member { get; set; }
        public virtual ICollection<OrderItem> OrderItems { get; set; }

        
    }
}
