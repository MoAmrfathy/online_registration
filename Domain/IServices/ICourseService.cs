using Domain.Entities;
using Shared.Domain;
using System.Collections.Generic;
using System.Threading.Tasks;
using ViewModel;

namespace Domain.IServices
{
    public interface ICourseService
    {
        public Task<ServiceReturn<CourseViewModel>> AddCourse(CourseViewModel courseViewModel);
        public Task<ServiceReturn<bool>> DeleteCourse(int courseId);
        public Task<ServiceReturn<List<CourseViewModel>>> GetCoursesByTermId(int termId);
    }
}
