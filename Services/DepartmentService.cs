using Domain.Entities;
using Domain.IServices;
using Microsoft.Extensions.Logging;
using Shared.Domain;
using System;
using System.Threading.Tasks;

namespace Services
{
    public class DepartmentService : IDepartmentService
    {
        private readonly ILogger<DepartmentService> _logger;
        private readonly IRepository<Department> _departmentRepository;

        public DepartmentService(ILogger<DepartmentService> logger, IRepository<Department> departmentRepository)
        {
            _logger = logger;
            _departmentRepository = departmentRepository;
        }

        public async Task<ServiceReturn<Department>> AddDepartment(Department department)
        {
            var serviceReturn = new ServiceReturn<Department>();

            try
            {
                await _departmentRepository.Insert(department);
                serviceReturn.Data = department;
            }
            catch (Exception ex)
            {
                serviceReturn.AddTechnicalInsertError();
                _logger.LogError($"Error adding department: {ex.Message}");
            }

            return serviceReturn;
        }

        public async Task<ServiceReturn<bool>> DeleteDepartment(int id)
        {
            var serviceReturn = new ServiceReturn<bool>();

            try
            {
                var department = await _departmentRepository.FindBy(d => d.D_id == id);
                if (department != null && department.Any())
                {
                    await _departmentRepository.Delete(id);
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
                _logger.LogError($"Error deleting department: {ex.Message}");
            }

            return serviceReturn;
        }
    }
}
