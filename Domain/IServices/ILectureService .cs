using System.Collections.Generic;
using System.Threading.Tasks;
using Shared.Domain;
using ViewModel;

namespace Domain.IServices
{
    public interface ILectureService
    {
       public Task<ServiceReturn<LectureViewModel>> AddLecture(LectureViewModel lectureViewModel);
        public Task<ServiceReturn<bool>> DeleteLecture(int lectureId);
        public Task<ServiceReturn<List<string>>> GetLectureNamesByCourseId(int courseId);
    }
}
