using Microsoft.AspNetCore.Mvc;
using ProjectFUEN.Models.EFModels;

namespace ProjectFUEN.Models.DTOs
{
    public class MembersDTO 
    {

        public int Id { get; set; }
        public string EmailAccount { get; set; }
        public string EncryptedPassword { get; set; }
        public string RealName { get; set; }
        public string NickName { get; set; }
        public DateTime? BirthOfDate { get; set; }
        public string Mobile { get; set; }
        public string Address { get; set; }
        public string PhotoSticker { get; set; }
        public string About { get; set; }
        public string ConfirmCode { get; set; }
        public bool IsConfirmed { get; set; }
        public bool IsInBlackList { get; set; }

        public virtual ICollection<OrderDetail> OrderDetails { get; set; }
        public virtual ICollection<ShoppingCart> ShoppingCarts { get; set; }

        public virtual ICollection<Coupon> Coupons { get; set; }
        public virtual ICollection<Product> Products { get; set; }
    }
}
