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
    public class PrerequisiteService : IPrerequisiteService
    {
        private readonly ILogger<PrerequisiteService> _logger;
        private readonly IRepository<Prerequisite> _prerequisiteRepository;
        private readonly IRepository<Course> _courseRepository;

        public PrerequisiteService(
            ILogger<PrerequisiteService> logger,
            IRepository<Prerequisite> prerequisiteRepository,
            IRepository<Course> courseRepository)
        {
            _logger = logger;
            _prerequisiteRepository = prerequisiteRepository;
            _courseRepository = courseRepository;
        }

        public async Task<ServiceReturn<PrerequisiteViewModel>> AddPrerequisite(PrerequisiteViewModel prerequisiteViewModel)
        {
            var serviceReturn = new ServiceReturn<PrerequisiteViewModel>();
            try
            {
                // Manually map PrerequisiteViewModel to Prerequisite
                var prerequisite = new Prerequisite
                {
                    PrerequisiteId = prerequisiteViewModel.PrerequisiteId,
                    C_id = prerequisiteViewModel.C_id,
                    RequiredCourseId = prerequisiteViewModel.RequiredCourseId,
                    // Add other property mappings as needed
                };

                // Insert the prerequisite entity into the repository
                await _prerequisiteRepository.Insert(prerequisite);

                // Manually map Prerequisite to PrerequisiteViewModel after the entity is saved
                serviceReturn.Data = new PrerequisiteViewModel
                {
                    PrerequisiteId = prerequisite.PrerequisiteId,
                    C_id = prerequisite.C_id,
                    RequiredCourseId = prerequisite.RequiredCourseId,
                    // Add other property mappings as needed
                };
            }
            catch (Exception ex)
            {
                // Log the error and add a technical error to the ServiceReturn
                serviceReturn.AddTechnicalInsertError();
                _logger.LogError($"Error adding prerequisite: {ex.Message}");
            }

            return serviceReturn;
        }


        public async Task<ServiceReturn<List<PrerequisiteViewModel>>> GetPrerequisitesByCourseId(int courseId)
        {
            var serviceReturn = new ServiceReturn<List<PrerequisiteViewModel>>();
            try
            {
                // Fetch prerequisites by the course ID
                var prerequisites = await _prerequisiteRepository.FindBy(p => p.C_id == courseId);

                // Manually map the list of Prerequisite entities to List<PrerequisiteViewModel>
                serviceReturn.Data = prerequisites.Select(p => new PrerequisiteViewModel
                {
                    PrerequisiteId = p.PrerequisiteId,
                    C_id = p.C_id,
                    RequiredCourseId = p.RequiredCourseId,
                    // Add other property mappings as needed
                }).ToList();
            }
            catch (Exception ex)
            {
                // Handle exceptions and log the error
                serviceReturn.AddTechnicalError();
                _logger.LogError($"Error retrieving prerequisites: {ex.Message}");
            }

            return serviceReturn;
        }

        public async Task<ServiceReturn<bool>> DeletePrerequisitesByCourseId(int courseId)
        {
            var serviceReturn = new ServiceReturn<bool>();
            try
            {
                var prerequisites = await _prerequisiteRepository.FindBy(p => p.C_id == courseId);
                if (prerequisites.Any())
                {
                    foreach (var prerequisite in prerequisites)
                    {
                        await _prerequisiteRepository.Delete(prerequisite.PrerequisiteId);
                    }
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
                _logger.LogError($"Error deleting prerequisites: {ex.Message}");
            }
            return serviceReturn;
        }

        // New method to get the course name of RequiredCourseId by course ID (C_id)
        public async Task<ServiceReturn<string>> GetRequiredCourseNameByCourseId(int courseId)
        {
            var serviceReturn = new ServiceReturn<string>();
            try
            {
                // Find the prerequisite entry for the specified C_id
                var prerequisite = await _prerequisiteRepository.FindBy(p => p.C_id == courseId);

                if (prerequisite.Any())
                {
                    var requiredCourseId = prerequisite.First().RequiredCourseId;

                    // Use RequiredCourseId to find the corresponding course name
                    var requiredCourse = await _courseRepository.FindBy(c => c.C_id == requiredCourseId);
                    if (requiredCourse.Any())
                    {
                        serviceReturn.Data = requiredCourse.First().C_Title; // Assuming C_Title is the course name
                    }
                    else
                    {
                        serviceReturn.AddError("Required course not found.");
                    }
                }
                else
                {
                    serviceReturn.AddError("No prerequisite found for the given course.");
                }
            }
            catch (Exception ex)
            {
                serviceReturn.AddTechnicalError();
                _logger.LogError($"Error retrieving required course name: {ex.Message}");
            }
            return serviceReturn;
        }
    }
}
