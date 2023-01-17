namespace ProjectFUEN.Models.ViewModels
{
    public class ProductIndexVM
    {
        public int Id { get; set; }
        public int CategoryId { get; set; }
        public string CategoryName { get; set; }
        public int BrandId { get; set; }
        public string BrandName { get; set; }
        public string Name { get; set; }
        public int Price { get; set; }
    }
}
