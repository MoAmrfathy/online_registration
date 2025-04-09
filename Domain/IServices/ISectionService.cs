using System.Collections.Generic;
using System.Threading.Tasks;
using Shared.Domain;
using ViewModel;

namespace Domain.IServices
{
    public interface ISectionService
    {
       public Task<ServiceReturn<SectionViewModel>> AddSection(SectionViewModel sectionViewModel);
       public Task<ServiceReturn<bool>> DeleteSection(int sectionId);
       public Task<ServiceReturn<List<string>>> GetSectionNamesByCourseId(int courseId);
    }
}
