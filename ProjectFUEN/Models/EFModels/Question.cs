﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable disable
using ProjectFUEN.Models.ViewModels;
using System;
using System.Collections.Generic;

namespace ProjectFUEN.Models.EFModels
{
    public partial class Question
    {
        public Question()
        {
            Answers = new HashSet<Answer>();
        }

        public int Id { get; set; }
        public string Content { get; set; }
        public DateTime DateCreated { get; set; }
        public int ActivityId { get; set; }
        public int MemberId { get; set; }
        public bool State { get; set; }

        public virtual Activity Activity { get; set; }
        public virtual Member Member { get; set; }
        public virtual ICollection<Answer> Answers { get; set; }
    }
    public static partial class QuestionExts
    {
        public static QaVM QToqaVM(this Question source)
        {
            if (source.Answers.Count == 0)
            {
                return new QaVM
                {
                    QuestionId = source.Id,
                    QuestionContent = source.Content,
                    QuestionDateCreated = source.DateCreated,
                    ActivityName = source.Activity.ActivityName,
                    ActivityId = source.ActivityId,
                    EmailAccount = source.Member.EmailAccount,
                    MemberId = source.Member.Id,
                    State = source.State,
                    AnswerId = null,
                    AnswerContent = null,
                    AnswerDateCreated =null
                };
            }
            else
            {
                return new QaVM
                {
                    QuestionId = source.Id,
                    QuestionContent = source.Content,
                    QuestionDateCreated = source.DateCreated,
                    ActivityName = source.Activity.ActivityName,
                    ActivityId = source.ActivityId,
                    EmailAccount = source.Member.EmailAccount,
                    MemberId = source.Member.Id,
                    State = source.State,
                    AnswerId = source.Answers.ToArray()[0].Id,
                    AnswerContent = source.Answers.ToArray()[0].Content,
                    AnswerDateCreated = source.Answers.ToArray()[0].DateCreated

                };
            }

        }
    }
}