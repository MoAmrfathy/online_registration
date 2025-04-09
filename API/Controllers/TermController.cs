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
    public class TermController : ControllerBase
    {
        private readonly ILogger<TermController> _logger;
        private readonly ITermService _termService;

        public TermController(ILogger<TermController> logger, ITermService termService)
        {
            _logger = logger;
            _termService = termService;
        }

        // Get terms by graduation plan name
        [HttpGet("GetTermsByGraduationPlanName/{graduationPlanName}")]
        public async Task<ServiceReturn<List<TermViewModel>>> GetTermsByGraduationPlanName(string graduationPlanName)
        {
            var apiReturn = new ServiceReturn<List<TermViewModel>>();

            try
            {
                var result = await _termService.GetTermsByGraduationPlanName(graduationPlanName);

                if (result.HasErrors)
                {
                    apiReturn.Errors = result.Errors;
                }
                else
                {
                    apiReturn.Data = result.Data.Adapt<List<TermViewModel>>(); 
                }
            }
            catch (Exception ex)
            {
                apiReturn.AddError($"Error getting terms: {ex.Message}");
            }

            return apiReturn;
        }

        // Add a new term
        [HttpPost("AddTerm")]
        public async Task<ServiceReturn<TermViewModel>> AddTerm(TermViewModel termViewModel)
        {
            var apiReturn = new ServiceReturn<TermViewModel>();

            try
            {
                var term = termViewModel.Adapt<Term>();
                var result = await _termService.AddTerm(term);

                if (result.HasErrors)
                {
                    apiReturn.Errors = result.Errors;
                }
                else
                {
                    apiReturn.Data = result.Data.Adapt<TermViewModel>();
                }
            }
            catch (Exception ex)
            {
                apiReturn.AddError($"Error adding term: {ex.Message}");
            }

            return apiReturn;
        }

        // Delete a term
        [HttpDelete("DeleteTerm/{termId}")]
        public async Task<ServiceReturn<bool>> DeleteTerm(int termId)
        {
            var apiReturn = new ServiceReturn<bool>();

            try
            {
                var result = await _termService.DeleteTerm(termId);
                apiReturn.Data = result.Data;

                if (result.HasErrors)
                {
                    apiReturn.Errors = result.Errors;
                }
            }
            catch (Exception ex)
            {
                apiReturn.AddError($"Error deleting term: {ex.Message}");
            }

            return apiReturn;
        }
    }
}
