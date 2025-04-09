using Domain.Entities;
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
    public class GraduationPlanController : ControllerBase
    {
        private readonly ILogger<GraduationPlanController> _logger;
        private readonly IGraduationPlanService _graduationPlanService;

        public GraduationPlanController(ILogger<GraduationPlanController> logger, IGraduationPlanService graduationPlanService)
        {
            _logger = logger;
            _graduationPlanService = graduationPlanService;
        }

        [HttpPost("AddGraduationPlan")]
        public async Task<ServiceReturn<GraduationPlanViewModel>> AddGraduationPlan(GraduationPlanViewModel graduationPlanViewModel)
        {
            var apiReturn = new ServiceReturn<GraduationPlanViewModel>();

            try
            {
                var graduationPlan = graduationPlanViewModel.Adapt<GraduationPlan>();
                var result = await _graduationPlanService.AddGraduationPlan(graduationPlan);

                if (result.HasErrors)
                {
                    apiReturn.Errors = result.Errors;
                }
                else
                {
                    apiReturn.Data = result.Data.Adapt<GraduationPlanViewModel>();
                }
            }
            catch (Exception ex)
            {
                apiReturn.AddError($"Error adding graduation plan: {ex.Message}");
            }

            return apiReturn;
        }

        [HttpGet("GetGraduationPlanByDepartmentName/{departmentName}")]
        public async Task<ServiceReturn<GraduationPlanViewModel>> GetGraduationPlanByDepartmentName(string departmentName)
        {
            var apiReturn = new ServiceReturn<GraduationPlanViewModel>();

            try
            {
                var result = await _graduationPlanService.GetGraduationPlanByDepartmentName(departmentName);

                if (result.HasErrors)
                {
                    apiReturn.Errors = result.Errors;
                }
                else
                {
                    apiReturn.Data = result.Data.Adapt<GraduationPlanViewModel>();
                }
            }
            catch (Exception ex)
            {
                apiReturn.AddError($"Error retrieving graduation plan: {ex.Message}");
            }

            return apiReturn;
        }

        [HttpDelete("DeleteGraduationPlan/{id}")]
        public async Task<ServiceReturn<bool>> DeleteGraduationPlan(int id)
        {
            var apiReturn = new ServiceReturn<bool>();

            try
            {
                var result = await _graduationPlanService.DeleteGraduationPlan(id);
                apiReturn.Data = result.Data;

                if (result.HasErrors)
                {
                    apiReturn.Errors = result.Errors;
                }
            }
            catch (Exception ex)
            {
                apiReturn.AddError($"Error deleting graduation plan: {ex.Message}");
            }

            return apiReturn;
        }
    }
}
