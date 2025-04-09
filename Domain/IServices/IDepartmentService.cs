using Domain.Entities;
using Shared.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Domain.IServices
{
    public interface IDepartmentService
    {
       public Task<ServiceReturn<Department>> AddDepartment(Department department);
        public Task<ServiceReturn<bool>> DeleteDepartment(int id);

    }
}
