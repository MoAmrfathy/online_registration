using Domain.Entities;
using Domain.IServices;
using Microsoft.Extensions.Logging;
using Shared.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Services
{
    public class TermService : ITermService
    {
        private readonly ILogger<TermService> _logger;
        private readonly IRepository<Term> _termRepository;
        private readonly IRepository<GraduationPlan> _graduationPlanRepository;

        public TermService(ILogger<TermService> logger, IRepository<Term> termRepository, IRepository<GraduationPlan> graduationPlanRepository)
        {
            _logger = logger;
            _termRepository = termRepository;
            _graduationPlanRepository = graduationPlanRepository;
        }

  
        public async Task<ServiceReturn<List<Term>>> GetTermsByGraduationPlanName(string graduationPlanName)
        {
            var serviceReturn = new ServiceReturn<List<Term>>();

            try
            {
  
                var graduationPlan = await _graduationPlanRepository
                    .FindBy(gp => gp.g_name.ToLower() == graduationPlanName.ToLower()); // Ensure case-insensitive comparison

                if (graduationPlan != null && graduationPlan.Any())
                {
                    var graduationPlanId = graduationPlan.First().GraduationPlanId;

   
                    var terms = await _termRepository.FindBy(t => t.GraduationPlanId == graduationPlanId);

                    if (terms != null && terms.Any())
                    {
                        serviceReturn.Data = terms.ToList();
                    }
                    else
                    {
                        serviceReturn.AddError("No terms found for the given graduation plan.");
                    }
                }
                else
                {
                    serviceReturn.AddError("Graduation plan not found.");
                }
            }
            catch (Exception ex)
            {
                serviceReturn.AddTechnicalError();
                _logger.LogError($"Error retrieving terms for graduation plan '{graduationPlanName}': {ex.Message}");
            }

            return serviceReturn;
        }

        public async Task<ServiceReturn<Term>> AddTerm(Term term)
        {
            var serviceReturn = new ServiceReturn<Term>();

            try
            {
            
                var graduationPlan = await _graduationPlanRepository.FindBy(gp => gp.GraduationPlanId == term.GraduationPlanId);

                if (graduationPlan == null || !graduationPlan.Any())
                {
                    serviceReturn.AddError("The specified GraduationPlanId does not exist.");
                    return serviceReturn;
                }

                if (string.IsNullOrEmpty(term.Term_name))
                {
                    serviceReturn.AddError("Term name cannot be null or empty.");
                    return serviceReturn;
                }

                await _termRepository.Insert(term);
                serviceReturn.Data = term;
            }
            catch (Exception ex)
            {
                serviceReturn.AddTechnicalInsertError();
                _logger.LogError($"Error adding term: {ex.Message}");
            }

            return serviceReturn;
        }


        public async Task<ServiceReturn<bool>> DeleteTerm(int termId)
        {
            var serviceReturn = new ServiceReturn<bool>();

            try
            {
                var term = await _termRepository.FindBy(t => t.TermId == termId);
                if (term != null && term.Any())
                {
                    await _termRepository.Delete(termId);
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
                _logger.LogError($"Error deleting term: {ex.Message}");
            }

            return serviceReturn;
        }
    }
}
