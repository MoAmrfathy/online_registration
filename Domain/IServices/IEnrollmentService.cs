using Shared.Domain;
using System.Collections.Generic;
using System.Threading.Tasks;
using ViewModel;


namespace Domain.IServices
{
    public interface IEnrollmentService
    {
        public Task<ServiceReturn<StudentViewModel>> LoginByRegNoAndPin(long regNo, long pin);
        public Task<ServiceReturn<StudentViewModel>> GetStudentDetails(int studentId);
        public Task<ServiceReturn<bool>> CheckEligibilityForEnrollment(int studentId);
        public Task<ServiceReturn<List<CourseViewModel>>> ListCoursesByStudentSemester(int studentId);
        public Task<ServiceReturn<List<GroupDetailsViewModel>>> ListGroupsByCourseId(int courseId);
        public Task<ServiceReturn<bool>> CheckPrerequisitesAndConflicts(int studentId);
        public void ClearSelectedCourses(List<CourseGroupPair> selectedCourses );
        public Task<ServiceReturn<bool>> RemoveCourseFromSelectedList(int selectedCourseId);
        public Task<ServiceReturn<bool>> AddEnrollment(int studentId);
        public Task<ServiceReturn<List<GroupDetailsViewModel>>> GetSelectedCoursesWithGroupDetails(int studentId);
        public Task<ServiceReturn<bool>> AddSelectedCourses(int studentId, List<CourseGroupPair> selectedCourses);
    }
}
