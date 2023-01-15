using ProjectFUEN.Models.EFModels;

namespace ProjectFUEN.Models.ViewModels
{
    public class ActivityMemberVM
    {
        public int ActivityId { get; set; }
        public string ActivityName { get; set; }
        public string EmailAccount { get; set; }
        public string RealName { get; set; }

        public string Mobile { get; set; }
        public DateTime DateJoined { get; set; }

    }
    public static partial class ActivityExts
    {
        public static ActivityMemberVM ToActivityMemberVM(this ActivityMember source)
        {
            return new ActivityMemberVM
            {
                ActivityId=source.ActivityId,
                ActivityName=source.Activity.ActivityName,
                EmailAccount=source.Member.EmailAccount,
                RealName=source.Member.RealName,
                Mobile=source.Member.Mobile,
                DateJoined=source.DateJoined

            };
        }


    }
}
