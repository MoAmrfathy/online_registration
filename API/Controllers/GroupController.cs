using Domain.IServices;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Shared.Domain;
using System;
using System.Threading.Tasks;
using ViewModel;

namespace API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class GroupController : ControllerBase
    {
        private readonly ILogger<GroupController> _logger;
        private readonly IGroupService _groupService;

        public GroupController(ILogger<GroupController> logger, IGroupService groupService)
        {
            _logger = logger;
            _groupService = groupService;
        }

        [HttpPost("AddGroup")]
        public async Task<ServiceReturn<GroupViewModel>> AddGroup(GroupViewModel groupViewModel)
        {
            var apiReturn = new ServiceReturn<GroupViewModel>();

            try
            {
                var result = await _groupService.AddGroup(groupViewModel);

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
                apiReturn.AddError($"Error adding group: {ex.Message}");
                _logger.LogError($"Error adding group: {ex.Message}");
            }

            return apiReturn;
        }

        [HttpGet("GetGroupDetails")]
        public async Task<ServiceReturn<GroupDetailsViewModel>> GetGroupDetails(int courseId, string groupName)
        {
            var apiReturn = new ServiceReturn<GroupDetailsViewModel>();

            try
            {
                var result = await _groupService.GetGroupDetails(courseId, groupName);

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
                apiReturn.AddError($"Error retrieving group details: {ex.Message}");
                _logger.LogError($"Error retrieving group details: {ex.Message}");
            }

            return apiReturn;
        }
        [HttpPost("IncreaseGroupCapacity")]
        public async Task<ServiceReturn<bool>> IncreaseGroupCapacity(int groupId)
        {
            var apiReturn = new ServiceReturn<bool>();

            try
            {
                var result = await _groupService.IncreaseGroupCapacity(groupId);

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
                apiReturn.AddError($"Error increasing group capacity: {ex.Message}");
                _logger.LogError($"Error increasing group capacity: {ex.Message}");
            }

            return apiReturn;
        }
    }
}
