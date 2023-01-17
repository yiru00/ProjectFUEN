using Microsoft.Build.Framework;

namespace ProjectFUEN.Models.ViewModels
{
    public class LoginVM
    {
        [System.ComponentModel.DataAnnotations.Required(ErrorMessage = "請輸入帳號")]
        public string Account { get; set; }
        [System.ComponentModel.DataAnnotations.Required(ErrorMessage = "請輸入密碼")]
        public string Password { get; set; }
    }
}
