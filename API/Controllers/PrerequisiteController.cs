using Domain.IServices;
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
    public class PrerequisiteController : ControllerBase
    {
        private readonly ILogger<PrerequisiteController> _logger;
        private readonly IPrerequisiteService _prerequisiteService;

        public PrerequisiteController(ILogger<PrerequisiteController> logger, IPrerequisiteService prerequisiteService)
        {
            _logger = logger;
            _prerequisiteService = prerequisiteService;
        }

        [HttpPost("AddPrerequisite")]
        public async Task<ServiceReturn<PrerequisiteViewModel>> AddPrerequisite(PrerequisiteViewModel prerequisiteViewModel)
        {
            var apiReturn = new ServiceReturn<PrerequisiteViewModel>();

            try
            {
                var result = await _prerequisiteService.AddPrerequisite(prerequisiteViewModel);

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
                apiReturn.AddError($"Error adding prerequisite: {ex.Message}");
            }

            return apiReturn;
        }

        [HttpGet("GetPrerequisitesByCourseId/{courseId}")]
        public async Task<ServiceReturn<List<PrerequisiteViewModel>>> GetPrerequisitesByCourseId(int courseId)
        {
            var apiReturn = new ServiceReturn<List<PrerequisiteViewModel>>();

            try
            {
                var result = await _prerequisiteService.GetPrerequisitesByCourseId(courseId);

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
                apiReturn.AddError($"Error retrieving prerequisites: {ex.Message}");
            }

            return apiReturn;
        }

        [HttpDelete("DeletePrerequisitesByCourseId/{courseId}")]
        public async Task<ServiceReturn<bool>> DeletePrerequisitesByCourseId(int courseId)
        {
            var apiReturn = new ServiceReturn<bool>();

            try
            {
                var result = await _prerequisiteService.DeletePrerequisitesByCourseId(courseId);
                apiReturn.Data = result.Data;

                if (result.HasErrors)
                {
                    apiReturn.Errors = result.Errors;
                }
            }
            catch (Exception ex)
            {
                apiReturn.AddError($"Error deleting prerequisites: {ex.Message}");
            }

            return apiReturn;
        }

        // New endpoint to get the course name of RequiredCourseId by course ID (C_id)
        [HttpGet("GetRequiredCourseNameByCourseId/{courseId}")]
        public async Task<ServiceReturn<string>> GetRequiredCourseNameByCourseId(int courseId)
        {
            var apiReturn = new ServiceReturn<string>();

            try
            {
                var result = await _prerequisiteService.GetRequiredCourseNameByCourseId(courseId);

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
                apiReturn.AddError($"Error retrieving required course name: {ex.Message}");
            }

            return apiReturn;
        }
    }
}
