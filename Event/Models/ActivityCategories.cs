﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable disable
using System;
using System.Collections.Generic;

namespace Event.Models
{
    public partial class ActivityCategories
    {
        public ActivityCategories()
        {
            Activities = new HashSet<Activities>();
        }

        public int Id { get; set; }
        public int DisplayOrder { get; set; }
        public string CategoryName { get; set; }

        public virtual ICollection<Activities> Activities { get; set; }
    }
}