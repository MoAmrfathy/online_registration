using Shared.Domain;
using System.Collections.Generic;
using System.Threading.Tasks;
using ViewModel;

namespace Domain.IServices
{
    public interface IStudentService
    {
       public Task<ServiceReturn<StudentViewModel>> AddStudent(StudentViewModel studentViewModel);
        public Task<ServiceReturn<List<StudentViewModel>>> GetStudentsByGraduationPlanName(string graduationPlanName);
    }
}
