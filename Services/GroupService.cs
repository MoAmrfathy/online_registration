using System;
using System.Linq;
using System.Threading.Tasks;
using Domain.Entities;
using Domain.IServices;
using Microsoft.Extensions.Logging;
using Shared.Domain;
using ViewModel;

namespace Services
{
    public class GroupService : IGroupService
    {
        private readonly ILogger<GroupService> _logger;
        private readonly IRepository<Group> _groupRepository;
        private readonly IRepository<Lecture> _lectureRepository;
        private readonly IRepository<Section> _sectionRepository;
        private readonly IRepository<Course> _courseRepository;

        public GroupService(ILogger<GroupService> logger, IRepository<Group> groupRepository,
                            IRepository<Lecture> lectureRepository, IRepository<Section> sectionRepository,
                            IRepository<Course> courseRepository)
        {
            _logger = logger;
            _groupRepository = groupRepository;
            _lectureRepository = lectureRepository;
            _sectionRepository = sectionRepository;
            _courseRepository = courseRepository;
        }

        public async Task<ServiceReturn<GroupViewModel>> AddGroup(GroupViewModel groupViewModel)
        {
            var serviceReturn = new ServiceReturn<GroupViewModel>();
            try
            {
                // Manually map GroupViewModel to Group
                var group = new Group
                {
                    GroupId = groupViewModel.GroupId,
                    GroupName = groupViewModel.GroupName,
                    LectureId = groupViewModel.LectureId,
                    SectionId = groupViewModel.SectionId,
                    Capacity = groupViewModel.Capacity,
                    // Add other property mappings as needed
                };

                // Insert the group entity into the repository
                await _groupRepository.Insert(group);

                // Manually map Group to GroupViewModel after the entity is saved
                serviceReturn.Data = new GroupViewModel
                {
                    GroupId = group.GroupId,
                    GroupName = group.GroupName,
                    LectureId = group.LectureId,
                    SectionId = group.SectionId,
                    Capacity = group.Capacity,
                    // Add other property mappings as needed
                };
            }
            catch (Exception ex)
            {
                // Log the error and add a technical error to the ServiceReturn
                serviceReturn.AddTechnicalInsertError();
                _logger.LogError($"Error adding group: {ex.Message}");
            }

            return serviceReturn;
        }


        public async Task<ServiceReturn<GroupDetailsViewModel>> GetGroupDetails(int courseId, string groupName)
        {
            var serviceReturn = new ServiceReturn<GroupDetailsViewModel>();
            try
            {
                var groups = await _groupRepository.FindBy(g => g.C_id == courseId && g.GroupName == groupName);
                var group = groups.FirstOrDefault();

                if (group != null)
                {
                    var lecture = (await _lectureRepository.FindBy(l => l.LectureId == group.LectureId)).FirstOrDefault();
                    var section = (await _sectionRepository.FindBy(s => s.SectionId == group.SectionId)).FirstOrDefault();
                    var course = (await _courseRepository.FindBy(c => c.C_id == courseId)).FirstOrDefault();

                    var groupDetails = new GroupDetailsViewModel
                    {
                        GroupName = group.GroupName,
                        LectureName = lecture?.LectureName,
                        SectionName = section?.SectionName,
                        LectureTime = $"{lecture?.Day} {lecture?.LectureStartTime} - {lecture?.LectureEndTime}",
                        SectionTime = $"{section?.Day} {section?.sectionStartTime} - {section?.sectionEndTime}",
                        CourseName = course?.C_Title
                    };
                    serviceReturn.Data = groupDetails;
                }
                else
                {
                    serviceReturn.AddNotFoundError();
                }
            }
            catch (Exception ex)
            {
                serviceReturn.AddTechnicalError();
                _logger.LogError($"Error retrieving group details: {ex.Message}");
            }
            return serviceReturn;
        }
        public async Task<ServiceReturn<bool>> IncreaseGroupCapacity(int groupId)
        {
            var serviceReturn = new ServiceReturn<bool>();
            try
            {
                // Fetch the group
                var group = (await _groupRepository.FindBy(g => g.GroupId == groupId)).FirstOrDefault();
                if (group == null)
                {
                    serviceReturn.AddError($"Group with ID {groupId} not found.");
                    serviceReturn.Data = false;
                    return serviceReturn;
                }

                // Increment the group's capacity
                group.Capacity += 1;

                // Update the group in the repository
                await _groupRepository.Update(group);

                serviceReturn.Data = true;
            }
            catch (Exception ex)
            {
                serviceReturn.AddTechnicalError();
                _logger.LogError($"Error increasing group capacity: {ex.Message}");
            }

            return serviceReturn;
        }

    }
}
