using Shared.UI;
using UI.Helpers;
using UI.HttpClientServices;
using ViewModel;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace UI.DataConsumers
{
    public class EnrollmentConsumer
    {
        private readonly ApiService _apiService;

        public EnrollmentConsumer(ApiService apiService)
        {
            _apiService = apiService;
        }

        public async Task<APIReturn<LoginResponse>> LoginByRegNoAndPin(long regNo, long pin)
        {
            return await _apiService.httpClient.GetJsonAsync<LoginResponse>(
                $"api/Enrollment/LoginByRegNoAndPin?regNo={regNo}&pin={pin}");
        }

        public async Task<APIReturn<bool>> CheckEligibilityForEnrollment()
        {
            return await _apiService.httpClient.GetJsonAsync<bool>("api/Enrollment/CheckEligibility");
        }

        public async Task<APIReturn<StudentViewModel>> GetStudentDetails()
        {
            return await _apiService.httpClient.GetJsonAsync<StudentViewModel>("api/Enrollment/GetStudentDetails");
        }

        public async Task<APIReturn<List<CourseViewModel>>> ListCoursesByStudentSemester()
        {
            return await _apiService.httpClient.GetJsonAsync<List<CourseViewModel>>(
                "api/Enrollment/ListCoursesBySemester");
        }

        public async Task<APIReturn<List<GroupDetailsViewModel>>> ListGroupsByCourseId(int courseId)
        {
            return await _apiService.httpClient.GetJsonAsync<List<GroupDetailsViewModel>>(
                $"api/Enrollment/ListGroupsByCourseId/{courseId}");
        }

        public async Task<APIReturn<bool>> CheckPrerequisitesAndConflicts(List<CourseGroupPair> selectedCourses)
        {
            return await _apiService.httpClient.PostJsonAsync<bool, List<CourseGroupPair>>(
                "api/Enrollment/CheckPrerequisites", selectedCourses);
        }

        public async Task<APIReturn<bool>> AddEnrollment(List<CourseGroupPair> selectedCourses)
        {
            return await _apiService.httpClient.PostJsonAsync<bool, List<CourseGroupPair>>(
                "api/Enrollment/AddEnrollment", selectedCourses);
        }

        public async Task<APIReturn<string>> ClearSelectedCourses(List<CourseGroupPair> selectedCourses)
        {
            return await _apiService.httpClient.PostJsonAsync<string, List<CourseGroupPair>>(
                "api/Enrollment/ClearSelectedCourses", selectedCourses);
        }

        public async Task<APIReturn<bool>> RemoveCourseFromSelectedList(int selectedCourseId)
        {
            var parameters = new { selectedCourseId };
            return await _apiService.httpClient.PostJsonAsync<bool, object>(
                "api/Enrollment/RemoveCourseFromSelectedList", parameters);
        }



        public async Task<APIReturn<List<GroupDetailsViewModel>>> GetSelectedCoursesWithGroupDetails(List<CourseGroupPair> selectedCourses)
        {
            return await _apiService.httpClient.PostJsonAsync<List<GroupDetailsViewModel>, List<CourseGroupPair>>(
                "api/Enrollment/GetSelectedCoursesWithGroupDetails", selectedCourses);
        }

        public async Task<APIReturn<bool>> AddSelectedCourses(List<CourseGroupPair> selectedCourses)
        {
            return await _apiService.httpClient.PostJsonAsync<bool, List<CourseGroupPair>>(
                "api/Enrollment/AddSelectedCourses", selectedCourses);
        }
    }
}
