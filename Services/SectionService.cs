using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Domain.Entities;
using Domain.IServices;
using Microsoft.Extensions.Logging;
using Shared.Domain;
using ViewModel;

namespace Services
{
    public class SectionService : ISectionService
    {
        private readonly ILogger<SectionService> _logger;
        private readonly IRepository<Section> _sectionRepository;

        public SectionService(ILogger<SectionService> logger, IRepository<Section> sectionRepository)
        {
            _logger = logger;
            _sectionRepository = sectionRepository;
        }

        public async Task<ServiceReturn<SectionViewModel>> AddSection(SectionViewModel sectionViewModel)
        {
            var serviceReturn = new ServiceReturn<SectionViewModel>();
            try
            {
                // Manually map SectionViewModel to Section
                var section = new Section
                {
                    SectionId = sectionViewModel.SectionId,
                    SectionName = sectionViewModel.SectionName,
                    Day = Enum.TryParse<DayOfWeek>(sectionViewModel.Day, true, out var day) ? day : throw new ArgumentException("Invalid DayOfWeek value"),
                    sectionStartTime = TimeSpan.TryParse(sectionViewModel.sectionStartTime, out var startTime) ? startTime : throw new ArgumentException("Invalid TimeSpan for SectionStartTime"),
                    sectionEndTime = TimeSpan.TryParse(sectionViewModel.sectionEndTime, out var endTime) ? endTime : throw new ArgumentException("Invalid TimeSpan for SectionEndTime"),
                 
                    // Add other property mappings as needed
                };

                // Insert the section entity into the repository
                await _sectionRepository.Insert(section);

                // Manually map Section to SectionViewModel after the entity is saved
                serviceReturn.Data = new SectionViewModel
                {
                    SectionId = section.SectionId,
                    SectionName = section.SectionName,
                    Day = section.Day.ToString(),
                    sectionStartTime = section.sectionStartTime.ToString(),
                    sectionEndTime = section.sectionEndTime.ToString(),
                    // Add other property mappings as needed
                };
            }
            catch (Exception ex)
            {
                // Handle exceptions and add a technical error to the ServiceReturn
                serviceReturn.AddTechnicalInsertError();
                _logger.LogError($"Error adding section: {ex.Message}");
            }

            return serviceReturn;
        }


        public async Task<ServiceReturn<bool>> DeleteSection(int sectionId)
        {
            var serviceReturn = new ServiceReturn<bool>();
            try
            {
                var section = await _sectionRepository.FindBy(s => s.SectionId == sectionId);
                if (section.Any())
                {
                    await _sectionRepository.Delete(sectionId);
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
                _logger.LogError($"Error deleting section: {ex.Message}");
            }
            return serviceReturn;
        }

        public async Task<ServiceReturn<List<string>>> GetSectionNamesByCourseId(int courseId)
        {
            var serviceReturn = new ServiceReturn<List<string>>();
            try
            {
                var sections = await _sectionRepository.FindBy(s => s.C_id == courseId);
                serviceReturn.Data = sections.Select(s => s.SectionName).ToList();
            }
            catch (Exception ex)
            {
                serviceReturn.AddTechnicalError();
                _logger.LogError($"Error retrieving section names: {ex.Message}");
            }
            return serviceReturn;
        }
    }
}
