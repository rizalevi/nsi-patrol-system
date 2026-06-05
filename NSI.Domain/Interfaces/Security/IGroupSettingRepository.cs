using NSI.Domain.DTOs.Security;
using NSI.Domain.Models.Security;

namespace NSI.Domain.Interfaces.Security
{
    public interface IGroupSettingRepository
    {
        Task<List<GroupPrivilegeDto>> GetAllPrivilege(string groupId);
        Task SavePrivilege(GroupPrivilege model, string userLogin);

        //course setting
        Task<List<CourseSettingDto>> GetAllCourseSetting(string groupId, string plantCode);
        Task SaveCourseSetting(CourseSetting model, string userLogin);

    }
}
