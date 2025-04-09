using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Domain.Entities;
using Domain.IServices;
using Microsoft.Extensions.Logging;
using Shared.Domain;
using ViewModel;

namespace Services
{
    public class CourseService : ICourseService
    {
        private readonly ILogger<CourseService> _logger;
        private readonly IRepository<Course> _courseRepository;

        public CourseService(ILogger<CourseService> logger, IRepository<Course> courseRepository)
        {
            _logger = logger;
            _courseRepository = courseRepository;
        }

        public async Task<ServiceReturn<CourseViewModel>> AddCourse(CourseViewModel courseViewModel)
        {
            var serviceReturn = new ServiceReturn<CourseViewModel>();
            try
            {

                var course = new Course
                {
                    C_id = courseViewModel.C_id,
                    TermId = courseViewModel.TermId,
                    C_code = courseViewModel.C_code,
                    C_Title = courseViewModel.C_Title,
                    Credits = courseViewModel.Credits

                };

                await _courseRepository.Insert(course);


                serviceReturn.Data = new CourseViewModel
                {
                    C_id = course.C_id,
                    TermId = course.TermId,
                    C_code = course.C_code,
                    C_Title = course.C_Title,

                };
            }
            catch (Exception ex)
            {
                serviceReturn.AddTechnicalInsertError();
                _logger.LogError($"Error adding course: {ex.Message}");
            }
            return serviceReturn;
        }

        public async Task<ServiceReturn<bool>> DeleteCourse(int courseId)
        {
            var serviceReturn = new ServiceReturn<bool>();
            try
            {
                var course = await _courseRepository.FindBy(c => c.C_id == courseId);
                if (course.Any())
                {
                    await _courseRepository.Delete(courseId);
                    serviceReturn.Data = true;
                }
                else
                {
                    serviceReturn.AddNotFoundError();
                }
            }
            catch (Exception ex)
            {
                serviceReturn.AddTechnicalDeleteError();
                _logger.LogError($"Error deleting course: {ex.Message}");
            }
            return serviceReturn;
        }

        public async Task<ServiceReturn<List<CourseViewModel>>> GetCoursesByTermId(int termId)
        {
            var serviceReturn = new ServiceReturn<List<CourseViewModel>>();
            try
            {
                var courses = await _courseRepository.FindBy(c => c.TermId == termId);

                // Manually map the list of Course to List<CourseViewModel>
                serviceReturn.Data = courses.Select(c => new CourseViewModel
                {
                    C_id = c.C_id,
                    TermId = c.TermId,
                    C_code = c.C_code,
                    C_Title = c.C_Title,
                    Credits = c.Credits
                    // Add other property mappings as needed
                }).ToList();
            }
            catch (Exception ex)
            {
                serviceReturn.AddTechnicalError();
                _logger.LogError($"Error retrieving courses: {ex.Message}");
            }
            return serviceReturn;
        }
    }
}
