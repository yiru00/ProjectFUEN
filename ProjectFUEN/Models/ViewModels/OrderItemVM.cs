﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable disable
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ProjectFUEN.Models.EFModels
{
    public partial class OrderItemVM
    {
        public int Id { get; set; }
        public int OrderId { get; set; }
        public int ProductId { get; set; }
        [Display(Name = "產品名稱")]
        public string ProductName { get; set; }
        [Display(Name = "產品價格")]
        public int ProductPrice { get; set; }
        [Display(Name = "產品數量")]
        public int ProductNumber { get; set; }

        public virtual OrderDetail Order { get; set; }
        public virtual Product Product { get; set; }
    }
}