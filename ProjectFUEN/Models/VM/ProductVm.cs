using ProjectFUEN.Models.EFModels;

namespace ProjectFUEN.Models.VM
{
    public class ProductVm
    {
        public int CategoryId { get; set; }
        public int BrandId { get; set; }
        //public string CategoryName { get; set; }
        //public string BrandName { get; set; }
        public int Id { get; set; }
        public string Name { get; set; }
        public int Price { get; set; }
        public DateTime ManufactorDate { get; set; }
        public int Inventory { get; set; }
        public string ProductSpec { get; set; }
        public IEnumerable<IFormFile> Sources { get; set; }

        public IEnumerable<String> FileNames { get; set; }
    }
}
