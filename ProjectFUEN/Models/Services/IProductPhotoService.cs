using ProjectFUEN.Models.Services.interfaces;

namespace ProjectFUEN.Models.Services
{
    public class IProductPhotoService
    {
        private readonly IProductPhotoRepository _repository;
        public IProductPhotoService(IProductPhotoRepository repository)
        {
            _repository = repository;
        }

    }
}
