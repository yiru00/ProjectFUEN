﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable disable
using System;
using System.Collections.Generic;

namespace ProjectFUEN.Models.EFModels
{
    public partial class Album
    {
        public Album()
        {
            AlbumItems = new HashSet<AlbumItem>();
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public int MemberId { get; set; }
        public DateTime CreatedTime { get; set; }

        public virtual Member Member { get; set; }
        public virtual ICollection<AlbumItem> AlbumItems { get; set; }
    }
}