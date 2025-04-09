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
    public class LectureController : ControllerBase
    {
        private readonly ILogger<LectureController> _logger;
        private readonly ILectureService _lectureService;

        public LectureController(ILogger<LectureController> logger, ILectureService lectureService)
        {
            _logger = logger;
            _lectureService = lectureService;
        }

        [HttpPost("AddLecture")]
        public async Task<ServiceReturn<LectureViewModel>> AddLecture(LectureViewModel lectureViewModel)
        {
            var apiReturn = new ServiceReturn<LectureViewModel>();

            try
            {
                var result = await _lectureService.AddLecture(lectureViewModel);

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
                apiReturn.AddError($"Error adding lecture: {ex.Message}");
            }

            return apiReturn;
        }

        [HttpDelete("DeleteLecture/{lectureId}")]
        public async Task<ServiceReturn<bool>> DeleteLecture(int lectureId)
        {
            var apiReturn = new ServiceReturn<bool>();

            try
            {
                var result = await _lectureService.DeleteLecture(lectureId);
                apiReturn.Data = result.Data;

                if (result.HasErrors)
                {
                    apiReturn.Errors = result.Errors;
                }
            }
            catch (Exception ex)
            {
                apiReturn.AddError($"Error deleting lecture: {ex.Message}");
            }

            return apiReturn;
        }

        [HttpGet("GetLectureNamesByCourseId/{courseId}")]
        public async Task<ServiceReturn<List<string>>> GetLectureNamesByCourseId(int courseId)
        {
            var apiReturn = new ServiceReturn<List<string>>();

            try
            {
                var result = await _lectureService.GetLectureNamesByCourseId(courseId);

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
                apiReturn.AddError($"Error retrieving lecture names: {ex.Message}");
            }

            return apiReturn;
        }
    }
}
