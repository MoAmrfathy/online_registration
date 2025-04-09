using Microsoft.Extensions.Logging;
using Shared.Domain;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ViewModel;
using Infrastructure;
using Domain.Entities;
using Domain.IServices;

namespace Services
{
    public class StudentService : IStudentService
    {
        private readonly IRepository<Student> _studentRepository;
        private readonly IRepository<Department> _departmentRepository;
        private readonly IRepository<GraduationPlan> _graduationPlanRepository;
        private readonly ILogger<StudentService> _logger;

        public StudentService(
            IRepository<Student> studentRepository,
            IRepository<GraduationPlan> graduationPlanRepository,
            IRepository<Department> departmentRepository,
            ILogger<StudentService> logger)
        {
            _studentRepository = studentRepository;
            _departmentRepository = departmentRepository;
            _graduationPlanRepository = graduationPlanRepository;
            _logger = logger;
        }


        public async Task<ServiceReturn<StudentViewModel>> AddStudent(StudentViewModel studentViewModel)
        {
            var serviceReturn = new ServiceReturn<StudentViewModel>();
            try
            {

                var department = (await _departmentRepository.FindBy(d => d.D_id == studentViewModel.D_id)).FirstOrDefault();
                if (department == null)
                {
                    serviceReturn.AddError($"Department with ID {studentViewModel.D_id} does not exist.");
                    return serviceReturn;
                }

                
                var graduationPlan = (await _graduationPlanRepository.FindBy(g => g.GraduationPlanId == studentViewModel.GraduationPlanId)).FirstOrDefault();
                if (graduationPlan == null)
                {
                    serviceReturn.AddError($"Graduation Plan with ID {studentViewModel.GraduationPlanId} does not exist.");
                    return serviceReturn;
                }

         
                var student = new Student
                {
                    S_id = studentViewModel.S_id,
                    PIN = studentViewModel.PIN,
                    Reg_no = studentViewModel.Reg_no,
                    S_Name = studentViewModel.S_Name, 
                    College = studentViewModel.College,
                    Semester = studentViewModel.Semester,
                    IsGraduate = studentViewModel.IsGraduate,
                    isfired = studentViewModel.isfired,
                    D_id = studentViewModel.D_id,
                    GraduationPlanId = studentViewModel.GraduationPlanId,
                    GPA = studentViewModel.GPA,
                    TotalCreditAchievement = studentViewModel.TotalCreditAchievement
                };

          
                await _studentRepository.Insert(student);

        
                serviceReturn.Data = new StudentViewModel
                {
                    S_id = student.S_id,
                    Reg_no = student.Reg_no,
                    Semester = student.Semester,
                    IsGraduate = student.IsGraduate,
                    College= student.College,
                    S_Name =student.S_Name,
                    isfired = student.isfired,
                    D_id = student.D_id,
                    D_name = department.D_name,
                    GraduationPlanId = student.GraduationPlanId,
                    GraduationPlanName = graduationPlan.g_name,
                    GPA = student.GPA,
                    TotalCreditAchievement = student.TotalCreditAchievement
                };
            }
            catch (Exception ex)
            {
                serviceReturn.AddTechnicalInsertError();
                _logger.LogError($"Error adding student: {ex.Message}");
            }

            return serviceReturn;
        }


        public async Task<ServiceReturn<bool>> DeleteStudent(int studentId)
        {
            var serviceReturn = new ServiceReturn<bool>();
            try
            {
                var student = (await _studentRepository.FindBy(s => s.S_id == studentId)).FirstOrDefault();

                if (student == null)
                {
                    serviceReturn.AddError("Student not found.");
                    return serviceReturn;
                }

                await _studentRepository.Delete(student.S_id); // Ensure Delete accepts a Student object
                serviceReturn.Data = true;
            }
            catch (Exception ex)
            {
                serviceReturn.AddTechnicalError();
                _logger.LogError($"Error deleting student: {ex.Message}");
            }

            return serviceReturn;
        }

        public async Task<ServiceReturn<List<StudentViewModel>>> GetAllNonGraduateStudents()
        {
            var serviceReturn = new ServiceReturn<List<StudentViewModel>>();
            try
            {
                var students = await _studentRepository.FindBy(s => !s.IsGraduate);

                serviceReturn.Data = students.Select(s => new StudentViewModel
                {
                    S_id = s.S_id,
                    Reg_no = s.Reg_no,
                    S_Name = s.S_Name,
                    College = s.College,
                    Semester = s.Semester,
                    IsGraduate = s.IsGraduate,
                    isfired = s.isfired,
                    D_id = s.D_id,
                    GraduationPlanId = s.GraduationPlanId,
                    GPA = s.GPA,
                    TotalCreditAchievement = s.TotalCreditAchievement
                }).ToList();
            }
            catch (Exception ex)
            {
                serviceReturn.AddTechnicalError();
                _logger.LogError($"Error retrieving non-graduate students: {ex.Message}");
            }

            return serviceReturn;
        }

        public async Task<ServiceReturn<StudentViewModel>> Login(long regNo, long pin)
        {
            var serviceReturn = new ServiceReturn<StudentViewModel>();
            try
            {
                var student = (await _studentRepository.FindBy(s => s.Reg_no == regNo && s.PIN == pin)).FirstOrDefault();

                if (student == null)
                {
                    serviceReturn.AddError("Invalid registration number or PIN.");
                }
                else
                {
                    var studentViewModel = new StudentViewModel
                    {
                        S_id = student.S_id,
                        Reg_no = student.Reg_no,
                        S_Name = student.S_Name,
                        College = student.College,
                        Semester = student.Semester,
                        IsGraduate = student.IsGraduate,
                        isfired = student.isfired,
                        D_id = student.D_id,
                        GraduationPlanId = student.GraduationPlanId,
                        GPA = student.GPA,
                        TotalCreditAchievement = student.TotalCreditAchievement
                    };

                    serviceReturn.Data = studentViewModel;
                }
            }
            catch (Exception ex)
            {
                serviceReturn.AddTechnicalError();
                _logger.LogError($"Error logging in student: {ex.Message}");
            }

            return serviceReturn;
        }

        public async Task<ServiceReturn<List<StudentViewModel>>> GetStudentsByGraduationPlanName(string graduationPlanName)
        {
            var serviceReturn = new ServiceReturn<List<StudentViewModel>>();
            try
            {
                // Retrieve graduation plan from GraduationPlan repository
                var graduationPlan = (await _graduationPlanRepository.FindBy(g => g.g_name == graduationPlanName)).FirstOrDefault();
                if (graduationPlan == null)
                {
                    serviceReturn.AddError($"Graduation Plan '{graduationPlanName}' not found.");
                    return serviceReturn;
                }

                // Retrieve students in the graduation plan
                var students = await _studentRepository.FindBy(s => s.GraduationPlanId == graduationPlan.GraduationPlanId);

                serviceReturn.Data = students.Select(s => new StudentViewModel
                {
                    S_id = s.S_id,
                    Reg_no = s.Reg_no,
                    S_Name = s.S_Name,
                    College = s.College,
                    Semester = s.Semester,
                    IsGraduate = s.IsGraduate,
                    isfired = s.isfired,
                    D_id = s.D_id,
                    GraduationPlanId = s.GraduationPlanId,
                    GPA = s.GPA,
                    TotalCreditAchievement = s.TotalCreditAchievement
                }).ToList();
            }
            catch (Exception ex)
            {
                serviceReturn.AddTechnicalError();
                _logger.LogError($"Error retrieving students by graduation plan: {ex.Message}");
            }

            return serviceReturn;
        }
    }
}
