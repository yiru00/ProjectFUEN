﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable disable
using System;
using System.Collections.Generic;

namespace Event.Models
{
    public partial class OrderDetails
    {
        public OrderDetails()
        {
            OrderItems = new HashSet<OrderItems>();
        }

        public int Id { get; set; }
        public int MemberId { get; set; }
        public DateTime OrderDate { get; set; }
        public string Address { get; set; }
        public int State { get; set; }

        public virtual Members Member { get; set; }
        public virtual ICollection<OrderItems> OrderItems { get; set; }
    }
}