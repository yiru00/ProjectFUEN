using ProjectFUEN.Models.EFModels;
using ProjectFUEN.Models.Services.interfaces;

namespace ProjectFUEN.Models.Infrastructures.Repositories
{
    public class ProductPhotoRepository: IProductPhotoRepository
    {
        private readonly ProjectFUENContext _db;

        public ProductPhotoRepository(ProjectFUENContext db)
        {
            _db = db;
        }
    }
}
