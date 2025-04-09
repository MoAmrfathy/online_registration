using Domain.IServices;
using Mapster;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using Shared.Domain;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using ViewModel;

namespace API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class EnrollmentController : ControllerBase
    {
        private readonly ILogger<EnrollmentController> _logger;
        private readonly IEnrollmentService _enrollmentService;
        private readonly IConfiguration _configuration;

        public EnrollmentController(ILogger<EnrollmentController> logger, IEnrollmentService enrollmentService, IConfiguration configuration)
        {
            _logger = logger;
            _enrollmentService = enrollmentService;
            _configuration = configuration;
        }


        [HttpGet("LoginByRegNoAndPin")]
        public async Task<ServiceReturn<LoginResponse>> Login(long regNo, long pin)
        {
            var apiReturn = new ServiceReturn<LoginResponse>();
            try
            {
                var result = await _enrollmentService.LoginByRegNoAndPin(regNo, pin);
                if (result.HasErrors)
                {
                    apiReturn.Errors = result.Errors;
                }
                else
                {
                    var tokenHandler = new JwtSecurityTokenHandler();
                    var key = Convert.FromHexString(_configuration["Jwt:Key"]);
                    var tokenDescriptor = new SecurityTokenDescriptor
                    {
                        Subject = new ClaimsIdentity(new[]
                        {
                            new Claim("StudentId", result.Data.S_id.ToString()),
                            new Claim("Name", result.Data.S_Name),
                            new Claim("Role", "Student")
                        }),

                        Expires = DateTime.UtcNow.AddSeconds(20000000),
                        Issuer = _configuration["Jwt:Issuer"],
                        Audience = _configuration["Jwt:Issuer"],
                        SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
                    };

                    var token = tokenHandler.CreateToken(tokenDescriptor);
                    var tokenString = tokenHandler.WriteToken(token);

                    apiReturn.Data = new LoginResponse { Student = result.Data, Token = tokenString };
                }
            }
            catch (Exception ex)
            {
                apiReturn.AddError($"Error: {ex.Message}");
            }
            return apiReturn;
        }

        [Authorize]
        [HttpGet("GetStudentDetails")]
        public async Task<ServiceReturn<StudentViewModel>> GetStudentDetails()
        {
            var apiReturn = new ServiceReturn<StudentViewModel>();
            try
            {
                var studentIdClaim = User.Claims.FirstOrDefault(c => c.Type == "StudentId");
                if (studentIdClaim == null)
                {
                    apiReturn.AddError("Student ID not found in token.");
                    return apiReturn;
                }

                if (!int.TryParse(studentIdClaim.Value, out var studentId))
                {
                    apiReturn.AddError("Invalid Student ID in token.");
                    return apiReturn;
                }

                var result = await _enrollmentService.GetStudentDetails(studentId);

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
                apiReturn.AddError($"Error retrieving student details: {ex.Message}");
            }

            return apiReturn;
        }



        [Authorize]
        [HttpGet("CheckEligibility")]
        public async Task<ServiceReturn<bool>> CheckEligibility()
        {
            var apiReturn = new ServiceReturn<bool>();

            try
            {
                var studentIdClaim = User.Claims.FirstOrDefault(c => c.Type == "StudentId");
                if (studentIdClaim == null)
                {
                    apiReturn.AddError("Student ID not found in token.");
                    return apiReturn;
                }

                if (!int.TryParse(studentIdClaim.Value, out var studentId))
                {
                    apiReturn.AddError("Invalid Student ID in token.");
                    return apiReturn;
                }

                var result = await _enrollmentService.CheckEligibilityForEnrollment(studentId);
                apiReturn.Data = result.Data;
                if (result.HasErrors)
                {
                    apiReturn.Errors = result.Errors;
                }
            }
            catch (Exception ex)
            {
                apiReturn.AddError($"Error checking eligibility: {ex.Message}");
            }

            return apiReturn;
        }

        [Authorize]
        [HttpPost("GetSelectedCoursesWithGroupDetails")]
        public async Task<ServiceReturn<List<GroupDetailsViewModel>>> GetSelectedCoursesWithGroupDetails()
        {
            var apiReturn = new ServiceReturn<List<GroupDetailsViewModel>>();

            try
            {
                var studentIdClaim = User.Claims.FirstOrDefault(c => c.Type == "StudentId");
                if (studentIdClaim == null || !int.TryParse(studentIdClaim.Value, out var studentId))
                {
                    apiReturn.AddError("Student ID not found in token.");
                    return apiReturn;
                }

                var result = await _enrollmentService.GetSelectedCoursesWithGroupDetails(studentId);

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
                apiReturn.AddError($"Error retrieving selected courses with group details: {ex.Message}");
                _logger.LogError($"Error retrieving selected courses with group details: {ex.Message}", ex);
            }

            return apiReturn;
        }




        [Authorize]
        [HttpGet("ListCoursesBySemester")]
        public async Task<ServiceReturn<List<CourseViewModel>>> ListCoursesBySemester()
        {
            var apiReturn = new ServiceReturn<List<CourseViewModel>>();
            try
            {
                var studentIdClaim = User.Claims.FirstOrDefault(c => c.Type == "StudentId");
                if (studentIdClaim == null || !int.TryParse(studentIdClaim.Value, out var studentId))
                {
                    apiReturn.AddError("Student ID not found in token.");
                    return apiReturn;
                }

                var result = await _enrollmentService.ListCoursesByStudentSemester(studentId);
                if (result.HasErrors)
                {
                    apiReturn.Errors = result.Errors;
                }
                else
                {
                    apiReturn.Data = result.Data.Select(c => c.Adapt<CourseViewModel>()).ToList();
                }
            }
            catch (Exception ex)
            {
                apiReturn.AddError($"Error: {ex.Message}");
            }
            return apiReturn;
        }

        [Authorize]
        [HttpPost("AddEnrollment")]
        public async Task<ServiceReturn<bool>> AddEnrollment()
        {
            var apiReturn = new ServiceReturn<bool>();

            try
            {
                var studentIdClaim = User.Claims.FirstOrDefault(c => c.Type == "StudentId");
                if (studentIdClaim == null || !int.TryParse(studentIdClaim.Value, out var studentId))
                {
                    apiReturn.AddError("Student ID not found in token.");
                    return apiReturn;
                }

                var result = await _enrollmentService.AddEnrollment(studentId);

                apiReturn.Data = result.Data;

                if (result.HasErrors)
                {
                    apiReturn.Errors = result.Errors;
                }
                else
                {
            
                    apiReturn.SuccessMessage = result.SuccessMessage;
                }
            }
            catch (Exception ex)
            {
                apiReturn.AddError($"Error: {ex.Message}");
                _logger.LogError($"Error in AddEnrollment: {ex.Message}", ex);
            }

            return apiReturn;
        }


        [Authorize]
        [HttpGet("ListGroupsByCourseId/{courseId}")]
        public async Task<ServiceReturn<List<GroupDetailsViewModel>>> ListGroupsByCourseId(int courseId)
        {
            var apiReturn = new ServiceReturn<List<GroupDetailsViewModel>>();
            try
            {
                var result = await _enrollmentService.ListGroupsByCourseId(courseId);
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
                apiReturn.AddError($"Error retrieving groups: {ex.Message}");
                _logger.LogError($"Error retrieving groups: {ex.Message}");
            }
            return apiReturn;
        }
        [Authorize]
        [HttpPost("CheckPrerequisites")]
        public async Task<ServiceReturn<bool>> CheckPrerequisites()
        {
            var apiReturn = new ServiceReturn<bool>();

            try
            {
          
                var studentIdClaim = User.Claims.FirstOrDefault(c => c.Type == "StudentId");
                if (studentIdClaim == null || !int.TryParse(studentIdClaim.Value, out var studentId))
                {
                    apiReturn.AddError("Student ID not found in token.");
                    return apiReturn;
                }

 
                var result = await _enrollmentService.CheckPrerequisitesAndConflicts(studentId);

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
                apiReturn.AddError($"Error checking prerequisites and conflicts: {ex.Message}");
                _logger.LogError($"Error checking prerequisites and conflicts: {ex.Message}", ex);
            }

            return apiReturn;
        }


        [Authorize]
        [HttpPost("RemoveCourseFromSelectedList")]
        public async Task<ServiceReturn<bool>> RemoveCourseFromSelectedList(int selectedCourseId)
        {
            var apiReturn = new ServiceReturn<bool>();

            try
            {
                var studentIdClaim = User.Claims.FirstOrDefault(c => c.Type == "StudentId");
                if (studentIdClaim == null || !int.TryParse(studentIdClaim.Value, out var studentId))
                {
                    apiReturn.AddError("Student ID not found in token.");
                    apiReturn.Data = false;
                    return apiReturn;
                }

                var serviceResult = await _enrollmentService.RemoveCourseFromSelectedList(selectedCourseId);

                if (serviceResult.Data)
                {
                    apiReturn.Data = true;
                    apiReturn.SuccessMessage = "Course removed successfully.";
                    _logger.LogInformation($"Course with SelectedCourseId {selectedCourseId} removed successfully for StudentId {studentId}.");
                }
                else
                {
                    apiReturn.Data = false; 
                    apiReturn.Errors = serviceResult.Errors;
                }
            }
            catch (Exception ex)
            {
                apiReturn.Data = false; 
                apiReturn.AddError($"Error removing course: {ex.Message}");
                _logger.LogError($"Error removing course with SelectedCourseId {selectedCourseId}: {ex.Message}", ex);
            }

            return apiReturn;
        }


        [Authorize]
        [HttpPost("AddSelectedCourses")]
        public async Task<ServiceReturn<bool>> AddSelectedCourses([FromBody] List<CourseGroupPair> selectedCourses)
        {
            var apiReturn = new ServiceReturn<bool>();

            try
            {
                var studentIdClaim = User.Claims.FirstOrDefault(c => c.Type == "StudentId");
                if (studentIdClaim == null || !int.TryParse(studentIdClaim.Value, out var studentId))
                {
                    apiReturn.AddError("Student ID not found in token.");
                    return apiReturn;
                }

                var result = await _enrollmentService.AddSelectedCourses(studentId, selectedCourses);

                apiReturn.Data = result.Data;
                if (result.HasErrors)
                {
                    apiReturn.Errors = result.Errors;
                }
            }
            catch (Exception ex)
            {
                apiReturn.AddError($"Error adding selected courses: {ex.Message}");
                _logger.LogError($"Error adding selected courses: {ex.Message}", ex);
            }

            return apiReturn;
        }


    }
}
