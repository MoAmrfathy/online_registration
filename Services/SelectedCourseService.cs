using System;
using System.Linq;
using System.Threading.Tasks;
using Domain.Entities;
using Domain.IServices;
using Microsoft.Extensions.Logging;
using Shared.Domain;
using ViewModel;

namespace Services
{
    public class SelectedCourseService : ISelectedCourseService
    {
        private readonly ILogger<SelectedCourseService> _logger;
        private readonly IRepository<SelectedCourse> _selectedCourseRepository;
        private readonly IRepository<Course> _courseRepository;
        private readonly IRepository<Student> _studentRepository;
        private readonly IRepository<Group> _groupRepository;

        public SelectedCourseService(
            ILogger<SelectedCourseService> logger,
            IRepository<SelectedCourse> selectedCourseRepository,
            IRepository<Course> courseRepository,
            IRepository<Student> studentRepository,
            IRepository<Group> groupRepository)
        {
            _logger = logger;
            _selectedCourseRepository = selectedCourseRepository;
            _courseRepository = courseRepository;
            _studentRepository = studentRepository;
            _groupRepository = groupRepository;
        }

        public async Task<ServiceReturn<SelectedCourseViewModel>> AddSelectedCourse(SelectedCourseViewModel selectedCourseViewModel)
        {
            var serviceReturn = new ServiceReturn<SelectedCourseViewModel>();

            try
            {
                // Validate Course
                var courseExists = (await _courseRepository.FindBy(c => c.C_id == selectedCourseViewModel.C_id)).Any();
                if (!courseExists)
                {
                    serviceReturn.AddError("Course does not exist.");
                    _logger.LogError($"Invalid CourseId: {selectedCourseViewModel.C_id}");
                    return serviceReturn;
                }

                // Validate Student
                _logger.LogInformation($"Checking existence for StudentId: {selectedCourseViewModel.S_id}");
                var studentExists = (await _studentRepository.FindBy(s => s.S_id == selectedCourseViewModel.S_id)).Any();
                if (!studentExists)
                {
                    serviceReturn.AddError("Student does not exist.");
                    _logger.LogError($"Invalid StudentId: {selectedCourseViewModel.S_id}");
                    return serviceReturn;
                }

                // Validate Group
                var groupExists = (await _groupRepository.FindBy(g => g.GroupId == selectedCourseViewModel.GroupId)).Any();
                if (!groupExists)
                {
                    serviceReturn.AddError("Group does not exist.");
                    _logger.LogError($"Invalid GroupId: {selectedCourseViewModel.GroupId}");
                    return serviceReturn;
                }

                // Create SelectedCourse
                var selectedCourse = new SelectedCourse
                {
                    C_id = selectedCourseViewModel.C_id,
                    S_id = selectedCourseViewModel.S_id,
                    GroupId = selectedCourseViewModel.GroupId,
                    SelectionDate = selectedCourseViewModel.SelectionDate
                };

                await _selectedCourseRepository.Insert(selectedCourse);

                serviceReturn.Data = new SelectedCourseViewModel
                {
                    C_id = selectedCourse.C_id,
                    S_id = selectedCourse.S_id,
                    GroupId = selectedCourse.GroupId,
                    SelectionDate = selectedCourse.SelectionDate
                };
            }
            catch (Exception ex)
            {
                serviceReturn.AddTechnicalInsertError();
                _logger.LogError($"Error adding selected course: {ex.Message}");
            }

            return serviceReturn;
        }

        public async Task<ServiceReturn<bool>> DeleteSelectedCourse(int id)
        {
            var serviceReturn = new ServiceReturn<bool>();

            try
            {
                var selectedCourse = await _selectedCourseRepository.FindBy(sc => sc.SelectedCourseId == id);

                if (selectedCourse != null && selectedCourse.Any())
                {
                    await _selectedCourseRepository.Delete(id);
                    serviceReturn.Data = true;
                }
                else
                {
                    serviceReturn.AddNotFoundError();
                    _logger.LogError($"SelectedCourseId {id} not found.");
                }
            }
            catch (Exception ex)
            {
                serviceReturn.AddTechnicalDeleteError();
                _logger.LogError($"Error deleting selected course: {ex.Message}");
            }

            return serviceReturn;
        }
    }
}
