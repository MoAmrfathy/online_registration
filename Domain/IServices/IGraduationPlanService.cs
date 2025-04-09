using Domain.Entities;
using Shared.Domain;
using System.Threading.Tasks;
using ViewModel;

namespace Domain.IServices
{
    public interface IGraduationPlanService
    {
       public Task<ServiceReturn<GraduationPlan>> AddGraduationPlan(GraduationPlan graduationPlan);
        public Task<ServiceReturn<GraduationPlan>> GetGraduationPlanByDepartmentName(string departmentName);
        public Task<ServiceReturn<bool>> DeleteGraduationPlan(int graduationPlanId);
    }
}
