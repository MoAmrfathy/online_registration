using System.Threading.Tasks;
using Shared.Domain;
using ViewModel;

namespace Domain.IServices
{
    public interface IGroupService
    {
        public Task<ServiceReturn<GroupViewModel>> AddGroup(GroupViewModel groupViewModel);
        public Task<ServiceReturn<GroupDetailsViewModel>> GetGroupDetails(int courseId, string groupName);
        public Task<ServiceReturn<bool>> IncreaseGroupCapacity(int groupId);
    }
}
