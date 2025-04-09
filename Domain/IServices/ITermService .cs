using Domain.Entities;
using Shared.Domain;
using System.Threading.Tasks;

namespace Domain.IServices
{
    public interface ITermService
    {
       public Task<ServiceReturn<List<Term>>> GetTermsByGraduationPlanName(string graduationPlanName);
       public  Task<ServiceReturn<Term>> AddTerm(Term term);
        public Task<ServiceReturn<bool>> DeleteTerm(int termId);
    }
}
