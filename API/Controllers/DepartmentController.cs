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
    public class DepartmentController : ControllerBase
    {
        private readonly ILogger<DepartmentController> _logger;
        private readonly IDepartmentService _departmentService;

        public DepartmentController(ILogger<DepartmentController> logger, IDepartmentService departmentService)
        {
            _logger = logger;
            _departmentService = departmentService;
        }

        [HttpPost("AddDepartment")]
        public async Task<ServiceReturn<DepartmentViewModel>> AddDepartment(DepartmentViewModel departmentViewModel)
        {
            var apiReturn = new ServiceReturn<DepartmentViewModel>();

            try
            {
                var department = departmentViewModel.Adapt<Department>();
                var result = await _departmentService.AddDepartment(department);

                if (result.HasErrors)
                {
                    apiReturn.Errors = result.Errors;
                }
                else
                {
                    apiReturn.Data = result.Data.Adapt<DepartmentViewModel>();
                }
            }
            catch (Exception ex)
            {
                apiReturn.AddError($"Error adding department: {ex.Message}");
            }

            return apiReturn;
        }

        [HttpDelete("DeleteDepartment/{id}")]
        public async Task<ServiceReturn<bool>> DeleteDepartment(int id)
        {
            var apiReturn = new ServiceReturn<bool>();

            try
            {
                var result = await _departmentService.DeleteDepartment(id);
                apiReturn.Data = result.Data;

                if (result.HasErrors)
                {
                    apiReturn.Errors = result.Errors;
                }
            }
            catch (Exception ex)
            {
                apiReturn.AddError($"Error deleting department: {ex.Message}");
            }

            return apiReturn;
        }
    }
}
