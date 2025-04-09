using Domain.Entities;
using Domain.IServices;
using Mapster;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Shared.Domain;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ViewModel;

namespace API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CourseController : ControllerBase
    {
        private readonly ILogger<CourseController> _logger;
        private readonly ICourseService _courseService;

        public CourseController(ILogger<CourseController> logger, ICourseService courseService)
        {
            _logger = logger;
            _courseService = courseService;
        }

        [HttpPost("AddCourse")]
        public async Task<ServiceReturn<CourseViewModel>> AddCourse(CourseViewModel courseViewModel)
        {
            var apiReturn = new ServiceReturn<CourseViewModel>();

            try
            {
                var result = await _courseService.AddCourse(courseViewModel);

                if (result.HasErrors)
                {
                    apiReturn.Errors = result.Errors;
                }
                else
                {
                    apiReturn.Data = result.Data;
                }
            }
            catch (Exception ex)
            {
                apiReturn.AddError($"Error adding course: {ex.Message}");
            }

            return apiReturn;
        }

        [HttpGet("GetCoursesByTermId/{termId}")]
        public async Task<ServiceReturn<List<CourseViewModel>>> GetCoursesByTermId(int termId)
        {
            var apiReturn = new ServiceReturn<List<CourseViewModel>>();

            try
            {
                var result = await _courseService.GetCoursesByTermId(termId);

                if (result.HasErrors)
                {
                    apiReturn.Errors = result.Errors;
                }
                else
                {
                    apiReturn.Data = result.Data;
                }
            }
            catch (Exception ex)
            {
                apiReturn.AddError($"Error retrieving courses: {ex.Message}");
            }

            return apiReturn;
        }

        [HttpDelete("DeleteCourse/{courseId}")]
        public async Task<ServiceReturn<bool>> DeleteCourse(int courseId)
        {
            var apiReturn = new ServiceReturn<bool>();

            try
            {
                var result = await _courseService.DeleteCourse(courseId);
                apiReturn.Data = result.Data;

                if (result.HasErrors)
                {
                    apiReturn.Errors = result.Errors;
                }
            }
            catch (Exception ex)
            {
                apiReturn.AddError($"Error deleting course: {ex.Message}");
            }

            return apiReturn;
        }
    }
}
