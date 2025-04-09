using Domain.IServices;
using Mapster;
using Microsoft.AspNetCore.Mvc;
using Shared.Domain;
using System.Threading.Tasks;
using ViewModel;

namespace API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class SelectedCourseController : ControllerBase
    {
        private readonly ILogger<SelectedCourseController> _logger;
        private readonly ISelectedCourseService _selectedCourseService;

        public SelectedCourseController(ILogger<SelectedCourseController> logger, ISelectedCourseService selectedCourseService)
        {
            _logger = logger;
            _selectedCourseService = selectedCourseService;
        }

        [HttpPost("AddSelectedCourse")]
        public async Task<ServiceReturn<SelectedCourseViewModel>> AddSelectedCourse(SelectedCourseViewModel selectedCourseViewModel)
        {
            var apiReturn = new ServiceReturn<SelectedCourseViewModel>();

            try
            {

                var result = await _selectedCourseService.AddSelectedCourse(selectedCourseViewModel);

                if (result.HasErrors)
                {
                    apiReturn.Errors = result.Errors;
                }
                else
                {
                    apiReturn.Data = result.Data.Adapt<SelectedCourseViewModel>();
                }
            }
            catch (Exception ex)
            {
                apiReturn.AddError($"Error adding selected course: {ex.Message}");
                _logger.LogError($"Error in AddSelectedCourse: {ex.Message}");
            }

            return apiReturn;
        }

        [HttpDelete("DeleteSelectedCourse/{id}")]
        public async Task<ServiceReturn<bool>> DeleteSelectedCourse(int id)
        {
            var apiReturn = new ServiceReturn<bool>();

            try
            {
                var result = await _selectedCourseService.DeleteSelectedCourse(id);
                apiReturn.Data = result.Data;

                if (result.HasErrors)
                {
                    apiReturn.Errors = result.Errors;
                }
            }
            catch (Exception ex)
            {
                apiReturn.AddError($"Error deleting selected course: {ex.Message}");
                _logger.LogError($"Error in DeleteSelectedCourse: {ex.Message}");
            }

            return apiReturn;
        }
    }
}
