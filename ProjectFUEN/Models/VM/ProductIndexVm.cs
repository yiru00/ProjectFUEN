using ProjectFUEN.Models.EFModels;

namespace ProjectFUEN.Models.VM
{
    public class ProductIndexVm
    {

        public string CategoryName { get; set; }
        public string BrandName { get; set; }
        public int Id { get; set; }
        public int CategoryId { get; set; }
        public int BrandId { get; set; }
        public string Name { get; set; }
        public int Price { get; set; }
        public DateTime ManufactorDate { get; set; }
        public DateTime ReleaseDate { get; set; }
        public int Inventory { get; set; }
        public string ProductSpec { get; set; }
        public virtual Product Product { get; set; }
        public virtual Brand Brand { get; set; }
        public string Source { get; set; }

 
        public virtual Category Category { get; set; }
        public virtual ProductPhoto ProductPhoto { get; set; }
        public virtual ICollection<OrderItem> OrderItems { get; set; }
        public virtual ICollection<ProductPhoto> ProductPhotos { get; set; }
        public virtual ICollection<ShoppingCart> ShoppingCarts { get; set; }

        public virtual ICollection<Event> Events { get; set; }
        public virtual ICollection<Member> Members { get; set; }


       
    }
}
