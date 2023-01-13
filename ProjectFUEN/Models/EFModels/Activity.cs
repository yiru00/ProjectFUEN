﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable disable
using ProjectFUEN.Models.ViewModels;
using System;
using System.Collections.Generic;

namespace ProjectFUEN.Models.EFModels
{
    public partial class Activity
    {
        public Activity()
        {
            ActivityCollections = new HashSet<ActivityCollection>();
            ActivityMembers = new HashSet<ActivityMember>();
            Questions = new HashSet<Question>();
        }

        public int Id { get; set; }
        public string CoverImage { get; set; }
        public string ActivityName { get; set; }
        public string Recommendation { get; set; }
        public string Address { get; set; }
        public int MemberLimit { get; set; }
        public string Description { get; set; }
        public DateTime GatheringTime { get; set; }
        public DateTime Deadline { get; set; }
        public DateTime DateOfCreated { get; set; }
        public int? InstructorId { get; set; }
        public int? CategoryId { get; set; }

        public virtual ActivityCategory Category { get; set; }
        public virtual Instructor Instructor { get; set; }
        public virtual ICollection<ActivityCollection> ActivityCollections { get; set; }
        public virtual ICollection<ActivityMember> ActivityMembers { get; set; }
        public virtual ICollection<Question> Questions { get; set; }
    }

    public static partial class ActivityExts
    {
        public static Activity ToEntity(this ActivityVM source)
        {
            return new Activity
            {
                Id = source.Id,
                CoverImage = source.CoverImage,
                ActivityName = source.ActivityName,
                Recommendation= source.Recommendation,
                Address = source.Address,
                MemberLimit = source.MemberLimit,
                Description = source.Description,
                GatheringTime= source.GatheringTime,
                Deadline = source.Deadline,
                DateOfCreated= source.DateOfCreated,
                InstructorId= source.InstructorId,
                CategoryId= source.CategoryId,


            };
        }
    }
}