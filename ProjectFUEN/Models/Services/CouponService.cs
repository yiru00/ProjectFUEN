
using Microsoft.EntityFrameworkCore;
using ProjectFUEN.Models.DTOs;
using ProjectFUEN.Models.Services.Interfaces;
using ProjectFUEN.Models.ViewModels;
using System.Net.Mail;
using System.Text;

namespace ProjectFUEN.Models.Services
{
    public class CouponService
    {
        private ICouponRepository repo;
        public CouponService(ICouponRepository repo) 
        {
            this.repo = repo;
        }

        public string SendCoupon(string couponCode)
        {
            MailMessage mail = new MailMessage();
            //前面是發信email後面是顯示的名稱
            mail.From = new MailAddress("projectfuen@gmail.com", "攝影交流平台");

            IEnumerable<string> emilAddresses = repo.GetAllEmails();

            StringBuilder sb = new StringBuilder();

            foreach (string email in emilAddresses)
            {
                sb.Append($"{email},");
            }

            string allEmails = sb.ToString().Substring(0, sb.Length - 1);

            //收信者email
            //mail.To.Add(email);
            mail.Bcc.Add(allEmails);

            //設定優先權
            mail.Priority = MailPriority.Normal;

            //標題
            mail.Subject = "恭喜您獲得優惠券！趕快來使用吧";

            string FilePath = Directory.GetCurrentDirectory() + "\\Views\\Templates\\MailContent.html";
            StreamReader str = new StreamReader(FilePath);
            string MailText = str.ReadToEnd();
            MailText = MailText.Replace("[CouponCode]", couponCode);


            str.Close();
            //內容
            mail.Body = MailText;

            //內容使用html
            mail.IsBodyHtml = true;

            //設定gmail的smtp (這是google的)
            SmtpClient MySmtp = new SmtpClient("smtp.gmail.com", 587);

            //您在gmail的帳號密碼
            MySmtp.Credentials = new System.Net.NetworkCredential("projectfuen@gmail.com", "qpzilvexdqnfzozc");

            //開啟ssl
            MySmtp.EnableSsl = true;

            //發送郵件
            MySmtp.Send(mail);

            //放掉宣告出來的MySmtp
            MySmtp = null;

            //放掉宣告出來的mail
            mail.Dispose();

            return "優惠碼已發送成功";
        }

        public (bool IsSuccess,string ErrorMsg) Create(CouponDto dto,int discountType)
        {
            int result = DiscountType(dto.Discount, discountType);

            if (result != 3)
            {
                string[] errmsg = { "您未選取折扣類型", "折扣必須是0.9~0.01的數字", "折價必須是整數且不能為0" };
                return (false, errmsg[result]);
            }

            repo.Create(dto);

            return (true, null);
        }

        public (bool IsSuccess, CouponDto data) CouponIsExist(int? id)
        {
            if (id == null) return (false, null);

            (bool IsExist, CouponDto data) response = repo.CouponIsExist(id);

            if(!response.IsExist) return (false, null);


            if (response.data.Discount >= 1) response.data.discountType = 2;
            else response.data.discountType = 1;

            return (true, response.data);
        }
        public (bool IsSuccess, string ErrorMsg) EditCoupon(CouponDto dto,int discountType)
        {
            (bool IsExist, CouponDto data) response = repo.CouponIsExist(dto.Id);

            if (!response.IsExist) return (false,null);

            int result = DiscountType(dto.Discount, discountType);

            if (result != 3)
            {
                string[] errmsg = { "您未選取折扣類型", "折扣必須是0.9~0.01的數字", "折價必須是整數且不能為0" };
                return (false, errmsg[result]);
            }

            repo.Edit(dto);

            return (true, null);
        }

        private int DiscountType(decimal discount, int discountType)
        {
            bool discountIsInt = int.TryParse(discount.ToString(), out int result);

            if (discountType == 1)
            {
                if (discount < 0.01m || discount >= 1 || discountIsInt) return 1;
            }
            else if (discountType == 2)
            {
                if (!discountIsInt || discount == 0) return 2;
            }
            else return 0;

            return 3;
        }
    }
}
