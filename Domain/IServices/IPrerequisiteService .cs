using System.Collections.Generic;
using System.Threading.Tasks;
using Shared.Domain;
using ViewModel;

namespace Domain.IServices
{
    public interface IPrerequisiteService
    {
       public Task<ServiceReturn<PrerequisiteViewModel>> AddPrerequisite(PrerequisiteViewModel prerequisiteViewModel);
        public Task<ServiceReturn<List<PrerequisiteViewModel>>> GetPrerequisitesByCourseId(int courseId);
        public Task<ServiceReturn<bool>> DeletePrerequisitesByCourseId(int courseId);
        public Task<ServiceReturn<string>> GetRequiredCourseNameByCourseId(int courseId);
    }
}
