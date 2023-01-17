using ProjectFUEN.Models.EFModels;
using ProjectFUEN.Models.ViewModels;

namespace ProjectFUEN.Models.Infrastructures.ExtensionMethods
{
    public static class MemberExtensions
    {
		public static MemberIndexVM EntityoIndexVM(this Member source)
		{
			return new MemberIndexVM()
			{
				Id = source.Id,
				EmailAccount = source.EmailAccount,
				RealName = (source.RealName != null) ? source.RealName : string.Empty,
				Mobile = (source.Mobile != null) ? source.Mobile : string.Empty,
				City = (source.Address != null) ? source.Address.Substring(0, 3) : string.Empty,
				BirthOfDate = (source.BirthOfDate.HasValue) ? source.BirthOfDate.Value.ToString("yyyy-MM-dd") : string.Empty,
			};
		}

		public static BlackListVM EntityToBlackVM(this Member source)
		{
			return new BlackListVM()
			{
				Id = source.Id,
				EmailAccount = source.EmailAccount,
				RealName = source.RealName
			};
		}
	}
}
