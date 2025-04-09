using Microsoft.Extensions.Logging;
using Shared.Domain;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ViewModel;
using Infrastructure;
using Domain.Entities;
using Domain.IServices;
using Microsoft.EntityFrameworkCore;

namespace Services
{
    public class EnrollmentService : IEnrollmentService
    {
        private readonly ILogger<EnrollmentService> _logger;
        private readonly ApplicationDataContext _dbContext;
        private readonly IRepository<Student> _studentRepository;
        private readonly IRepository<Enrollment> _enrollmentRepository;
        private readonly IRepository<Course> _courseRepository;
        private readonly IRepository<Prerequisite> _prerequisiteRepository;
        private readonly IRepository<Group> _groupRepository;
        private readonly IRepository<Lecture> _lectureRepository;
        private readonly IRepository<Section> _sectionRepository;
        private readonly IRepository<Term> _termRepository;
        private readonly IRepository<Department> _departmentRepository;
        private readonly IRepository<GraduationPlan> _graduationPlanRepository;
        private readonly IRepository<SelectedCourse> _selectedCourseRepository;


        public EnrollmentService(
            ILogger<EnrollmentService> logger,
            ApplicationDataContext dbContext,
            IRepository<Student> studentRepository,
            IRepository<Enrollment> enrollmentRepository,
            IRepository<Course> courseRepository,
            IRepository<Prerequisite> prerequisiteRepository,
            IRepository<Group> groupRepository,
            IRepository<Lecture> lectureRepository,
            IRepository<Section> sectionRepository,
            IRepository<Department> departmentRepository,
            IRepository<GraduationPlan> graduationPlanRepository,
            IRepository<Term> termRepository,
            IRepository<SelectedCourse> selectedCourseRepository
            )

        {
            _logger = logger;
            _dbContext = dbContext;
            _studentRepository = studentRepository;
            _enrollmentRepository = enrollmentRepository;
            _courseRepository = courseRepository;
            _prerequisiteRepository = prerequisiteRepository;
            _groupRepository = groupRepository;
            _lectureRepository = lectureRepository;
            _sectionRepository = sectionRepository;
            _termRepository = termRepository;
            _departmentRepository = departmentRepository;
            _graduationPlanRepository = graduationPlanRepository;
            _selectedCourseRepository = selectedCourseRepository;

        }



        public async Task<ServiceReturn<StudentViewModel>> LoginByRegNoAndPin(long regNo, long pin)
        {
            var serviceReturn = new ServiceReturn<StudentViewModel>();
            try
            {
                var student = (await _studentRepository.FindBy(s => s.Reg_no == regNo && s.PIN == pin)).FirstOrDefault();

                if (student == null)
                {
                    serviceReturn.AddError("Invalid registration number or PIN.");
                }
                else
                {

                    var term = (await _termRepository.FindBy(t => t.TermId == student.Semester)).FirstOrDefault();


                    var department = (await _departmentRepository.FindBy(d => d.D_id == student.D_id)).FirstOrDefault();


                    var graduationPlan = (await _graduationPlanRepository.FindBy(g => g.GraduationPlanId == student.GraduationPlanId)).FirstOrDefault();


                    var studentViewModel = new StudentViewModel
                    {
                        S_id = student.S_id,
                        Reg_no = student.Reg_no,
                        Semester = student.Semester,
                        IsGraduate = student.IsGraduate,
                        College = student.College,
                        S_Name = student.S_Name,
                        isfired = student.isfired,
                        TermName = term.Term_name,
                        D_name = department.D_name,
                        GraduationPlanName = graduationPlan.g_name,
                        TotalCreditAchievement = student.TotalCreditAchievement,
                        GPA = student.GPA,
                    };

                    serviceReturn.Data = studentViewModel;
                }
            }
            catch (Exception ex)
            {
                serviceReturn.AddTechnicalError();
                _logger.LogError($"Error logging in student: {ex.Message}");
            }

            return serviceReturn;
        }

        public async Task<ServiceReturn<StudentViewModel>> GetStudentDetails(int studentId)
        {
            var serviceReturn = new ServiceReturn<StudentViewModel>();
            try
            {
                var student = (await _studentRepository.FindBy(s => s.S_id == studentId)).FirstOrDefault();

                if (student == null)
                {
                    serviceReturn.AddError("Student not found.");
                    return serviceReturn;
                }

                var term = (await _termRepository.FindBy(t => t.TermId == student.Semester)).FirstOrDefault();
                var department = (await _departmentRepository.FindBy(d => d.D_id == student.D_id)).FirstOrDefault();
                var graduationPlan = (await _graduationPlanRepository.FindBy(g => g.GraduationPlanId == student.GraduationPlanId)).FirstOrDefault();

                var studentViewModel = new StudentViewModel
                {
                    Reg_no = student.Reg_no,
                    Semester = student.Semester,
                    College = student.College,
                    S_Name = student.S_Name,
                    TermName = term.Term_name,
                    D_name = department.D_name,
                    GraduationPlanName = graduationPlan.g_name,
                    TotalCreditAchievement = student.TotalCreditAchievement,
                    GPA = student.GPA
                };

                serviceReturn.Data = studentViewModel;
            }
            catch (Exception ex)
            {
                serviceReturn.AddTechnicalError();
                _logger.LogError($"Error retrieving student details: {ex.Message}");
            }

            return serviceReturn;
        }


        private readonly bool RegistrationAccess = true;
        public async Task<ServiceReturn<bool>> CheckEligibilityForEnrollment(int studentId)
        {
            var serviceReturn = new ServiceReturn<bool>();
            try
            {
                if (!RegistrationAccess)
                {
                    serviceReturn.Data = false;
                    serviceReturn.AddRegistrationDateError();
                    return serviceReturn;
                }

                var student = (await _studentRepository.FindBy(s => s.S_id == studentId)).FirstOrDefault();

                if (student == null)
                {
                    serviceReturn.Data = false;
                    serviceReturn.AddError("Student not found.");
                    _logger.LogError("Student not found.");
                }
                else if (student.IsGraduate)
                {
                    serviceReturn.Data = false;
                    serviceReturn.AddGraduateError();
                }
                else if (student.isfired)
                {
                    serviceReturn.Data = false;
                    serviceReturn.AddfiredError();
                }
                else
                {
                    serviceReturn.Data = true;
                }
            }
            catch (Exception ex)
            {
                serviceReturn.AddTechnicalError();
                _logger.LogError($"Error checking enrollment eligibility: {ex.Message}");
            }

            return serviceReturn;
        }



        public async Task<ServiceReturn<List<CourseViewModel>>> ListCoursesByStudentSemester(int studentId)
        {
            var serviceReturn = new ServiceReturn<List<CourseViewModel>>();
            try
            {


                var student = (await _studentRepository.FindBy(s => s.S_id == studentId)).FirstOrDefault();

                if (student == null)
                {

                    serviceReturn.AddError("Student not found.");
                    return serviceReturn;
                }


                var courses = await _courseRepository.FindBy(c => c.TermId == student.Semester);


                serviceReturn.Data = courses.Select(c => new CourseViewModel
                {
                    C_id = c.C_id,
                    TermId = c.TermId,
                    C_code = c.C_code,
                    C_Title = c.C_Title,
                    Credits = c.Credits,

                }).ToList();
            }
            catch (Exception ex)
            {

                serviceReturn.AddTechnicalError();
                _logger.LogError($"Error listing courses: {ex.Message}");
            }

            return serviceReturn;
        }

        public async Task<ServiceReturn<List<GroupDetailsViewModel>>> GetSelectedCoursesWithGroupDetails(int studentId)
        {
            var serviceReturn = new ServiceReturn<List<GroupDetailsViewModel>>();

            try
            {
                var student = (await _studentRepository.FindBy(s => s.S_id == studentId)).FirstOrDefault();
                if (student == null)
                {
                    serviceReturn.AddError("Invalid student ID.");
                    return serviceReturn;
                }

              
                var selectedCourses = (await _selectedCourseRepository.FindBy(sc => sc.S_id == studentId)).ToList();

        
                var courseIds = selectedCourses.Select(sc => sc.C_id).Distinct().ToList();
                var groupIds = selectedCourses.Select(sc => sc.GroupId).Distinct().ToList();

             
                var courses = (await _courseRepository.FindBy(c => courseIds.Contains(c.C_id))).ToList();
                var groups = (await _groupRepository.FindBy(g => groupIds.Contains(g.GroupId))).ToList();
                var lectures = (await _lectureRepository.All()).ToList();
                var sections = (await _sectionRepository.All()).ToList();
                var terms = (await _termRepository.All()).ToList();

                var groupDetails = new List<GroupDetailsViewModel>();

       
                foreach (var selectedCourse in selectedCourses)
                {
                    var course = courses.FirstOrDefault(c => c.C_id == selectedCourse.C_id);
                    var group = groups.FirstOrDefault(g => g.GroupId == selectedCourse.GroupId);

                    if (course == null || group == null)
                    {
                        serviceReturn.AddError($"Invalid course or group details for Course ID {selectedCourse.C_id}, Group ID {selectedCourse.GroupId}.");
                        continue;
                    }

                    var lecture = lectures.FirstOrDefault(l => l.LectureId == group.LectureId);
                    var section = sections.FirstOrDefault(s => s.SectionId == group.SectionId);
                    var term = terms.FirstOrDefault(t => t.TermId == course.TermId);

                    groupDetails.Add(new GroupDetailsViewModel
                    {
                        SelectedCourseId = selectedCourse.SelectedCourseId, 
                        GroupName = group.GroupName,
                        LectureName = lecture?.LectureName ?? "N/A",
                        SectionName = section?.SectionName ?? "N/A",
                        LectureTime = lecture != null ? $"{lecture.Day} {lecture.LectureStartTime} - {lecture.LectureEndTime}" : "N/A",
                        SectionTime = section != null ? $"{section.Day} {section.sectionStartTime} - {section.sectionEndTime}" : "N/A",
                        CourseName = course.C_Title,
                        TermName = term?.Term_name ?? "N/A",
                        CourseCode = course.C_code,
                        CreditHours = course.Credits
                    });
                }

              
                serviceReturn.Data = groupDetails;
            }
            catch (Exception ex)
            {
             
                serviceReturn.AddTechnicalError();
                _logger.LogError($"Error retrieving selected courses with group details for Student ID {studentId}: {ex.Message}", ex);
            }

            return serviceReturn;
        }






        public async Task<ServiceReturn<List<GroupDetailsViewModel>>> ListGroupsByCourseId(int courseId)
        {
            var serviceReturn = new ServiceReturn<List<GroupDetailsViewModel>>();
            try
            {
                var groups = await _groupRepository.FindBy(g => g.C_id == courseId);

                var groupDetails = new List<GroupDetailsViewModel>();
                foreach (var group in groups)
                {
                    var lecture = (await _lectureRepository.FindBy(l => l.LectureId == group.LectureId)).FirstOrDefault();
                    var section = (await _sectionRepository.FindBy(s => s.SectionId == group.SectionId)).FirstOrDefault();
                    var course = (await _courseRepository.FindBy(c => c.C_id == courseId)).FirstOrDefault();

                    var term = course != null ? (await _termRepository.FindBy(t => t.TermId == course.TermId)).FirstOrDefault() : null;

                    var groupDetail = new GroupDetailsViewModel
                    {
                        CourseId = courseId,
                        GroupId = group.GroupId,
                        GroupName = group.GroupName,
                        LectureName = lecture?.LectureName ?? "N/A",
                        SectionName = section?.SectionName ?? "N/A",
                        LectureTime = lecture != null ? $"{lecture.Day} {lecture.LectureStartTime} - {lecture.LectureEndTime}" : "N/A",
                        SectionTime = section != null ? $"{section.Day} {section.sectionStartTime} - {section.sectionEndTime}" : "N/A",
                        CourseName = course?.C_Title ?? "N/A",
                        TermName = term?.Term_name ?? "N/A",
                        CourseCode = course?.C_code ?? "N/A",
                        CreditHours = course?.Credits ?? 0
                    };

                    groupDetails.Add(groupDetail);
                }

                serviceReturn.Data = groupDetails;
            }
            catch (Exception ex)
            {
                serviceReturn.AddTechnicalError();
                _logger.LogError($"Error listing groups for course ID {courseId}: {ex.Message}");
            }
            return serviceReturn;
        }

        public async Task<ServiceReturn<bool>> CheckPrerequisitesAndConflicts(int studentId)
        {
            var serviceReturn = new ServiceReturn<bool>();

            try
            {
                // Step 1: Retrieve student's active (not completed) enrollments
                var studentEnrollments = (await _enrollmentRepository.FindBy(e => e.S_id == studentId && !e.IsCompleted)).ToList();

                // Step 2: Retrieve student information
                var student = (await _studentRepository.FindBy(s => s.S_id == studentId)).FirstOrDefault();
                if (student == null)
                {
                    serviceReturn.AddError("Student not found.");
                    serviceReturn.Data = false;
                    return serviceReturn;
                }

                // Step 3: Retrieve selected courses for the student
                var selectedCourses = (await _selectedCourseRepository.FindBy(sc => sc.S_id == studentId)).ToList();


                // Step 4: Calculate total credit hours and validate courses
                int totalCreditHours = 0;

                foreach (var selectedCourse in selectedCourses)
                {
                    var course = (await _courseRepository.FindBy(c => c.C_id == selectedCourse.C_id)).FirstOrDefault();
                    if (course == null)
                    {
                        serviceReturn.AddError($"Course with ID {selectedCourse.C_id} not found.");
                        serviceReturn.Data = false;
                        return serviceReturn;
                    }

                    totalCreditHours += course.Credits;

                    // Check if total credit hours exceed the maximum limit
                    if (totalCreditHours > 9)
                    {
                        serviceReturn.AddError("The total credit hours exceed the maximum limit of 9. Please adjust your selection.");
                        serviceReturn.Data = false;
                        return serviceReturn;
                    }

                    // Check prerequisites
                    var prerequisites = (await _prerequisiteRepository.FindBy(p => p.C_id == selectedCourse.C_id && p.RequiredCourseId != null)).ToList();
                    foreach (var prerequisite in prerequisites)
                    {
                        var enrollment = studentEnrollments.FirstOrDefault(e => e.C_id == prerequisite.RequiredCourseId);
                        if (enrollment == null || !enrollment.IsCompleted)
                        {
                            var requiredCourse = (await _courseRepository.FindBy(c => c.C_id == prerequisite.RequiredCourseId)).FirstOrDefault();
                            var requiredCourseName = requiredCourse?.C_Title ?? "the required course";
                            serviceReturn.AddError($"You can't register for the course as you did not complete the course {requiredCourseName}.");
                            serviceReturn.Data = false;
                            return serviceReturn;
                        }
                    }

                    // Check group capacity
                    var group = (await _groupRepository.FindBy(g => g.GroupId == selectedCourse.GroupId)).FirstOrDefault();
                    if (group == null)
                    {
                        serviceReturn.AddError($"Selected group with ID {selectedCourse.GroupId} not found.");
                        serviceReturn.Data = false;
                        return serviceReturn;
                    }

                    var currentEnrollmentCount = (await _enrollmentRepository.FindBy(e => e.GroupId == selectedCourse.GroupId)).Count();
                    if (currentEnrollmentCount >= group.MaxCapacity)
                    {
                        serviceReturn.AddError($"You can't register for this course as the group {selectedCourse.GroupId} is full. Max capacity: {group.MaxCapacity}.");
                        serviceReturn.Data = false;
                        return serviceReturn;
                    }
                }

                // Step 5: Check for time conflicts between selected courses
                for (int i = 0; i < selectedCourses.Count; i++)
                {
                    var selectedCourseA = selectedCourses[i];
                    var groupA = (await _groupRepository.FindBy(g => g.GroupId == selectedCourseA.GroupId)).FirstOrDefault();
                    var lectureA = (await _lectureRepository.FindBy(l => l.LectureId == groupA.LectureId)).FirstOrDefault();
                    var sectionA = (await _sectionRepository.FindBy(s => s.SectionId == groupA.SectionId)).FirstOrDefault();

                    for (int j = i + 1; j < selectedCourses.Count; j++)
                    {
                        var selectedCourseB = selectedCourses[j];
                        var groupB = (await _groupRepository.FindBy(g => g.GroupId == selectedCourseB.GroupId)).FirstOrDefault();
                        var lectureB = (await _lectureRepository.FindBy(l => l.LectureId == groupB.LectureId)).FirstOrDefault();
                        var sectionB = (await _sectionRepository.FindBy(s => s.SectionId == groupB.SectionId)).FirstOrDefault();

                        if ((lectureA != null && lectureB != null &&
                             lectureA.Day == lectureB.Day &&
                             lectureA.LectureStartTime < lectureB.LectureEndTime &&
                             lectureA.LectureEndTime > lectureB.LectureStartTime) ||

                            (sectionA != null && sectionB != null &&
                             sectionA.Day == sectionB.Day &&
                             sectionA.sectionStartTime < sectionB.sectionEndTime &&
                             sectionA.sectionEndTime > sectionB.sectionStartTime) ||

                            (lectureA != null && sectionB != null &&
                             lectureA.Day == sectionB.Day &&
                             lectureA.LectureStartTime < sectionB.sectionEndTime &&
                             lectureA.LectureEndTime > sectionB.sectionStartTime) ||

                            (sectionA != null && lectureB != null &&
                             sectionA.Day == lectureB.Day &&
                             sectionA.sectionStartTime < lectureB.LectureEndTime &&
                             sectionA.sectionEndTime > lectureB.LectureStartTime))
                        {
                            serviceReturn.AddError($"Time conflict: Group {groupA.GroupId} conflicts with group {groupB.GroupId}.");
                            serviceReturn.Data = false;
                            return serviceReturn;
                        }
                    }
                }

                serviceReturn.Data = true;
            }
            catch (Exception ex)
            {
                serviceReturn.AddTechnicalError();
                _logger.LogError($"Error checking prerequisites or conflicts: {ex.Message}");
            }

            return serviceReturn;
        }



        public void ClearSelectedCourses(List<CourseGroupPair> selectedCourses)
        {
            if (selectedCourses == null)
            {
                throw new ArgumentNullException(nameof(selectedCourses), "The selected courses list cannot be null.");
            }

            selectedCourses.Clear();
            _logger.LogInformation("Selected courses list has been cleared.");
        }


        public async Task<ServiceReturn<bool>> RemoveCourseFromSelectedList(int selectedCourseId)
        {
            var serviceReturn = new ServiceReturn<bool>();

            try
            {
                var selectedCourse = (await _selectedCourseRepository.FindBy(sc => sc.SelectedCourseId == selectedCourseId)).FirstOrDefault();

                if (selectedCourse == null)
                {
                    serviceReturn.AddError($"No course found with SelectedCourseId {selectedCourseId}. Please ensure the ID is correct.");
                    serviceReturn.Data = false;
                    return serviceReturn;
                }

                _dbContext.SelectedCourses.Remove(selectedCourse);
                await _dbContext.SaveChangesAsync();

                serviceReturn.Data = true;
                serviceReturn.SuccessMessage = $"Course with SelectedCourseId {selectedCourseId} has been successfully removed.";
                _logger.LogInformation($"Course with SelectedCourseId {selectedCourseId} removed successfully.");
            }
            catch (Exception ex)
            {
                serviceReturn.AddTechnicalError();
                serviceReturn.Data = false; 
                _logger.LogError($"An error occurred while removing course with SelectedCourseId {selectedCourseId}: {ex.Message}", ex);
            }

            return serviceReturn;
        }






        public async Task<ServiceReturn<bool>> AddEnrollment(int studentId)
        {
            var serviceReturn = new ServiceReturn<bool>();

            try
            {
                var student = (await _studentRepository.FindBy(s => s.S_id == studentId)).FirstOrDefault();
                if (student == null)
                {
                    serviceReturn.AddError("Student not found. Please ensure the provided student ID is correct.");
                    return serviceReturn;
                }

                var selectedCourses = (await _selectedCourseRepository.FindBy(sc => sc.S_id == studentId)).ToList();
                int totalCreditHours = 0;
                var studentEnrollments = (await _enrollmentRepository.FindBy(e => e.S_id == studentId)).ToList();

                foreach (var selectedCourse in selectedCourses)
                {
                    var course = (await _courseRepository.FindBy(c => c.C_id == selectedCourse.C_id)).FirstOrDefault();
                    if (course == null)
                    {
                        serviceReturn.AddError($"Course with ID {selectedCourse.C_id} not found.");
                        serviceReturn.Data = false;
                        return serviceReturn;
                    }

                    totalCreditHours += course.Credits;

                    if (totalCreditHours > 9)
                    {
                        serviceReturn.AddError("The total credit hours exceed the maximum limit of 9. Please adjust your selection and try again.");
                        serviceReturn.Data = false;
                        return serviceReturn;
                    }

                    var existingEnrollment = studentEnrollments.FirstOrDefault(e =>
                        e.C_id == selectedCourse.C_id && e.GroupId == selectedCourse.GroupId);

                    if (existingEnrollment != null)
                    {
                        serviceReturn.AddError($"You are already enrolled in Course ID {selectedCourse.C_id} for Group ID {selectedCourse.GroupId}.");
                        serviceReturn.Data = false;
                        return serviceReturn;
                    }

                    var group = (await _groupRepository.FindBy(g => g.GroupId == selectedCourse.GroupId)).FirstOrDefault();
                    if (group == null)
                    {
                        serviceReturn.AddError($"Group with ID {selectedCourse.GroupId} not found.");
                        serviceReturn.Data = false;
                        return serviceReturn;
                    }

                    var currentEnrollmentCount = (await _enrollmentRepository.FindBy(e => e.GroupId == group.GroupId)).Count();
                    if (currentEnrollmentCount >= group.MaxCapacity)
                    {
                        serviceReturn.AddError($"Group {group.GroupId} is full. Maximum capacity is {group.MaxCapacity}. Please select another group.");
                        serviceReturn.Data = false;
                        return serviceReturn;
                    }

                    var lecture = (await _lectureRepository.FindBy(l => l.LectureId == group.LectureId)).FirstOrDefault();
                    var section = (await _sectionRepository.FindBy(s => s.SectionId == group.SectionId)).FirstOrDefault();

                    foreach (var enrollment in studentEnrollments)
                    {
                        var enrolledGroup = (await _groupRepository.FindBy(g => g.GroupId == enrollment.GroupId)).FirstOrDefault();
                        var enrolledLecture = (await _lectureRepository.FindBy(l => l.LectureId == enrolledGroup.LectureId)).FirstOrDefault();
                        var enrolledSection = (await _sectionRepository.FindBy(s => s.SectionId == enrolledGroup.SectionId)).FirstOrDefault();

                        if ((lecture != null && enrolledLecture != null &&
                             lecture.Day == enrolledLecture.Day &&
                             lecture.LectureStartTime < enrolledLecture.LectureEndTime &&
                             lecture.LectureEndTime > enrolledLecture.LectureStartTime) ||

                            (section != null && enrolledSection != null &&
                             section.Day == enrolledSection.Day &&
                             section.sectionStartTime < enrolledSection.sectionEndTime &&
                             section.sectionEndTime > enrolledSection.sectionStartTime) ||

                            (lecture != null && enrolledSection != null &&
                             lecture.Day == enrolledSection.Day &&
                             lecture.LectureStartTime < enrolledSection.sectionEndTime &&
                             lecture.LectureEndTime > enrolledSection.sectionStartTime) ||

                            (section != null && enrolledLecture != null &&
                             section.Day == enrolledLecture.Day &&
                             section.sectionStartTime < enrolledLecture.LectureEndTime &&
                             section.sectionEndTime > enrolledLecture.LectureStartTime))
                        {
                            serviceReturn.AddError($"Time conflict detected: Course ID {selectedCourse.C_id} conflicts with another course you are already enrolled in.");
                            serviceReturn.Data = false;
                            return serviceReturn;
                        }
                    }

                    var enrollmentToAdd = new Enrollment
                    {
                        S_id = studentId,
                        C_id = selectedCourse.C_id,
                        GroupId = selectedCourse.GroupId,
                        GraduationPlanId = student.GraduationPlanId,
                        IsCompleted = false
                    };

                    await _enrollmentRepository.Insert(enrollmentToAdd);

                    group.Capacity += 1;
                    await _groupRepository.Update(group);
                }

                var temporaryRegistrationEndDate = DateTime.Now.AddDays(30);
                _logger.LogInformation("Registration completed successfully. Payments must be completed before this date {EndDate}.", temporaryRegistrationEndDate);

                await _dbContext.SaveChangesAsync();

                
                serviceReturn.SuccessMessage = "Registration completed successfully! Please complete payment before the specified deadline.";
                serviceReturn.Data = true;
            }
            catch (Exception ex)
            {
                serviceReturn.AddTechnicalInsertError();
                _logger.LogError($"An error occurred while adding enrollment for Student ID {studentId}: {ex.Message}");
            }

            return serviceReturn;
        }



        public async Task<ServiceReturn<bool>> AddSelectedCourses(int studentId, List<CourseGroupPair> selectedCourses)
        {
            var serviceReturn = new ServiceReturn<bool>();

            try
            {
                if (selectedCourses == null || !selectedCourses.Any())
                {
                    serviceReturn.AddError("No selected courses provided.");
                    serviceReturn.Data = false;
                    return serviceReturn;
                }

                var student = (await _studentRepository.FindBy(s => s.S_id == studentId)).FirstOrDefault();
                if (student == null)
                {
                    serviceReturn.AddError("Student not found.");
                    serviceReturn.Data = false;
                    return serviceReturn;
                }

                foreach (var courseGroup in selectedCourses)
                {
 
                    var courseExists = (await _courseRepository.FindBy(c => c.C_id == courseGroup.C_id)).Any();
                    var groupExists = (await _groupRepository.FindBy(g => g.GroupId == courseGroup.GroupId)).Any();

                    if (!courseExists)
                    {
                        serviceReturn.AddError($"Course with ID {courseGroup.C_id} does not exist.");
                        continue;
                    }

                    if (!groupExists)
                    {
                        serviceReturn.AddError($"Group with ID {courseGroup.GroupId} does not exist.");
                        continue;
                    }

 
                    var alreadyExists = (await _selectedCourseRepository.FindBy(sc =>
                        sc.S_id == studentId && sc.C_id == courseGroup.C_id && sc.GroupId == courseGroup.GroupId)).Any();

                    if (alreadyExists)
                    {
                        serviceReturn.AddError($"Course ID {courseGroup.C_id} and Group ID {courseGroup.GroupId} are already selected.");
                        continue;
                    }


                    var selectedCourse = new SelectedCourse
                    {
                        S_id = studentId,
                        C_id = courseGroup.C_id,
                        GroupId = courseGroup.GroupId,
                        SelectionDate = DateTime.UtcNow 
                    };

                    await _selectedCourseRepository.Insert(selectedCourse);
                }

                await _dbContext.SaveChangesAsync();

                serviceReturn.Data = true;
            }
            catch (Exception ex)
            {
                serviceReturn.AddTechnicalError();
                _logger.LogError($"Error adding selected courses: {ex.Message}");
            }

            return serviceReturn;
        }

    }
}


