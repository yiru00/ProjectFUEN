﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable disable
using ProjectFUEN.Models.ViewModels;
using System;
using System.Collections.Generic;

namespace ProjectFUEN.Models.EFModels
{
    public partial class Instructor
    {
        public Instructor()
        {
            Activities = new HashSet<Activity>();
        }

        public int Id { get; set; }
        public string InstructorName { get; set; }
        public string ResumePhoto { get; set; }
        public string Description { get; set; }

        public virtual ICollection<Activity> Activities { get; set; }
    }
    public static partial class InstructorExts
    {
        public static Instructor ToEntity(this InstructorVM source)
        {
            return new Instructor
            {
                Id = source.Id,
                InstructorName = source.InstructorName,
                ResumePhoto = source.ResumePhoto,
                Description = source.Description,

            };
        }
    }
}