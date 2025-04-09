using Domain.Entities;
using Domain.IServices;
using Microsoft.Extensions.Logging;
using Shared.Domain;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Services
{
    public class GraduationPlanService : IGraduationPlanService
    {
        private readonly ILogger<GraduationPlanService> _logger;
        private readonly IRepository<GraduationPlan> _graduationPlanRepository;
        private readonly IRepository<Department> _departmentRepository;

        public GraduationPlanService(ILogger<GraduationPlanService> logger, IRepository<GraduationPlan> graduationPlanRepository, IRepository<Department> departmentRepository)
        {
            _logger = logger;
            _graduationPlanRepository = graduationPlanRepository;
            _departmentRepository = departmentRepository;
        }

        public async Task<ServiceReturn<GraduationPlan>> AddGraduationPlan(GraduationPlan graduationPlan)
        {
            var serviceReturn = new ServiceReturn<GraduationPlan>();

            try
            {
                await _graduationPlanRepository.Insert(graduationPlan);
                serviceReturn.Data = graduationPlan;
            }
            catch (Exception ex)
            {
                serviceReturn.AddTechnicalInsertError();
                _logger.LogError($"Error adding graduation plan: {ex.Message}");
            }

            return serviceReturn;
        }

        public async Task<ServiceReturn<GraduationPlan>> GetGraduationPlanByDepartmentName(string departmentName)
        {
            var serviceReturn = new ServiceReturn<GraduationPlan>();

            try
            {
                var department = await _departmentRepository.FindBy(d => d.D_name == departmentName);
                if (department != null && department.Any())
                {
                    var graduationPlan = await _graduationPlanRepository.FindBy(g => g.D_id == department.First().D_id);
                    if (graduationPlan != null)
                    {
                        serviceReturn.Data = graduationPlan.First();
                    }
                    else
                    {
                        serviceReturn.AddNotFoundError();
                    }
                }
                else
                {
                    serviceReturn.AddNotFoundError();
                }
            }
            catch (Exception ex)
            {
                serviceReturn.AddTechnicalError();
                _logger.LogError($"Error retrieving graduation plan: {ex.Message}");
            }

            return serviceReturn;
        }

        public async Task<ServiceReturn<bool>> DeleteGraduationPlan(int graduationPlanId)
        {
            var serviceReturn = new ServiceReturn<bool>();

            try
            {
                var graduationPlan = await _graduationPlanRepository.FindBy(g => g.GraduationPlanId == graduationPlanId);
                if (graduationPlan != null && graduationPlan.Any())
                {
                    await _graduationPlanRepository.Delete(graduationPlanId);
                    serviceReturn.Data = true;
                }
                else
                {
                    serviceReturn.AddNotFoundError();
                }
            }
            catch (Exception ex)
            {
                serviceReturn.AddTechnicalDeleteError();
                _logger.LogError($"Error deleting graduation plan: {ex.Message}");
            }

            return serviceReturn;
        }
    }
}
