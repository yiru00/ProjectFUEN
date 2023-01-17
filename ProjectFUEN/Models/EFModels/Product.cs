﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable disable
using System;
using System.Collections.Generic;

namespace ProjectFUEN.Models.EFModels
{
    public partial class Product
    {
        public Product()
        {
            OrderItems = new HashSet<OrderItem>();
            ProductPhotos = new HashSet<ProductPhoto>();
            ShoppingCarts = new HashSet<ShoppingCart>();
            Events = new HashSet<Event>();
            Members = new HashSet<Member>();
        }

        public int Id { get; set; }
        public int CategoryId { get; set; }
        public int BrandId { get; set; }
        public string Name { get; set; }
        public int Price { get; set; }
        public DateTime ManufactorDate { get; set; }
        public DateTime ReleaseDate { get; set; }
        public int Inventory { get; set; }
        public string ProductSpec { get; set; }

        public virtual Brand Brand { get; set; }
        public virtual Category Category { get; set; }
        public virtual ICollection<OrderItem> OrderItems { get; set; }
        public virtual ICollection<ProductPhoto> ProductPhotos { get; set; }
        public virtual ICollection<ShoppingCart> ShoppingCarts { get; set; }

        public virtual ICollection<Event> Events { get; set; }
        public virtual ICollection<Member> Members { get; set; }
    }
}