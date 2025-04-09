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
    public class SectionController : ControllerBase
    {
        private readonly ILogger<SectionController> _logger;
        private readonly ISectionService _sectionService;

        public SectionController(ILogger<SectionController> logger, ISectionService sectionService)
        {
            _logger = logger;
            _sectionService = sectionService;
        }

        [HttpPost("AddSection")]
        public async Task<ServiceReturn<SectionViewModel>> AddSection(SectionViewModel sectionViewModel)
        {
            var apiReturn = new ServiceReturn<SectionViewModel>();

            try
            {
                var result = await _sectionService.AddSection(sectionViewModel);

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
                apiReturn.AddError($"Error adding section: {ex.Message}");
            }

            return apiReturn;
        }

        [HttpDelete("DeleteSection/{sectionId}")]
        public async Task<ServiceReturn<bool>> DeleteSection(int sectionId)
        {
            var apiReturn = new ServiceReturn<bool>();

            try
            {
                var result = await _sectionService.DeleteSection(sectionId);
                apiReturn.Data = result.Data;

                if (result.HasErrors)
                {
                    apiReturn.Errors = result.Errors;
                }
            }
            catch (Exception ex)
            {
                apiReturn.AddError($"Error deleting section: {ex.Message}");
            }

            return apiReturn;
        }

        [HttpGet("GetSectionNamesByCourseId/{courseId}")]
        public async Task<ServiceReturn<List<string>>> GetSectionNamesByCourseId(int courseId)
        {
            var apiReturn = new ServiceReturn<List<string>>();

            try
            {
                var result = await _sectionService.GetSectionNamesByCourseId(courseId);

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
                apiReturn.AddError($"Error retrieving section names: {ex.Message}");
            }

            return apiReturn;
        }
    }
}
