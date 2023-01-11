using ProjectFUEN.Models.EFModels;

namespace ProjectFUEN.Models.VM
{
    public class ProductPhotoVM
    {
        public int Id { get; set; }
        public int ProductId { get; set; }
        public string Source { get; set; }

        public virtual Product Product { get; set; }
    }
}
