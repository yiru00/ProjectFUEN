using ProjectFUEN.Models.Services.Interfaces;

namespace ProjectFUEN.Models.Services
{
    public class InstructorService
    {
        private readonly IInstructorRepository _repository;
        public InstructorService(IInstructorRepository repository)
        {
            _repository = repository;
        }

    }
}
