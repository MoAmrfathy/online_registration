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
    public class StudentController : ControllerBase
    {
        private readonly ILogger<StudentController> _logger;
        private readonly IStudentService _studentService;

        public StudentController(ILogger<StudentController> logger, IStudentService studentService)
        {
            _logger = logger;
            _studentService = studentService;
        }

        [HttpPost("AddStudent")]
        public async Task<ServiceReturn<StudentViewModel>> AddStudent(StudentViewModel studentViewModel)
        {
            var apiReturn = new ServiceReturn<StudentViewModel>();

            try
            {
                var result = await _studentService.AddStudent(studentViewModel);

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
                apiReturn.AddError($"Error adding student: {ex.Message}");
                _logger.LogError($"Error adding student: {ex.Message}");
            }

            return apiReturn;
        }

        [HttpGet("GetStudentsByGraduationPlanName/{graduationPlanName}")]
        public async Task<ServiceReturn<List<StudentViewModel>>> GetStudentsByGraduationPlanName(string graduationPlanName)
        {
            var apiReturn = new ServiceReturn<List<StudentViewModel>>();

            try
            {
                var result = await _studentService.GetStudentsByGraduationPlanName(graduationPlanName);

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
                apiReturn.AddError($"Error retrieving students by graduation plan name: {ex.Message}");
                _logger.LogError($"Error retrieving students by graduation plan name: {ex.Message}");
            }

            return apiReturn;
        }
    }
}
