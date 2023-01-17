using ProjectFUEN.Models.EFModels;
using ProjectFUEN.Models.Services.Interfaces;

namespace ProjectFUEN.Models.Infrastructures.Repositories
{
    
    public class InstructorRepository: IInstructorRepository
    {
        private readonly ProjectFUENContext _db;

        public InstructorRepository(ProjectFUENContext db)
        {
            _db = db;
        }
    }
}
