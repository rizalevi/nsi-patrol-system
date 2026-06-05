using NSI.Domain.DTOs.Master;
using NSI.Domain.DTOs.Security;
using NSI.Domain.Interfaces.Security;
using NSI.Domain.Models.Security;
using NSI.Infra.Datas;

namespace NSI.Infra.Repositories.Security
{
    public class GroupSettingRepository : IGroupSettingRepository
    {
        private readonly DbExecutor _dbExecutor;
       
        public GroupSettingRepository(DbExecutor dbExecutor)
        {
            _dbExecutor = dbExecutor;
        }

        public async Task<List<GroupPrivilegeDto>> GetAllPrivilege(string groupId)
        {
            string sp = "sp_Web_SS_GroupPrivilege_List";
            return (await _dbExecutor.QueryListAsync<GroupPrivilegeDto>(sp, new { GroupID = groupId })).ToList();
        }


        public async Task<List<CourseSettingDto>> GetAllCourseSetting(string groupId, string plantCode)
        {
            string sp = "sp_Web_SS_CourseSetting_List";
            return (await _dbExecutor.QueryListAsync<CourseSettingDto>(sp, new { GroupID = groupId , PlantCode = plantCode  })).ToList();
        }

        public Task SavePrivilege(GroupPrivilege model, string userLogin)
        {
            throw new NotImplementedException();
        }
        public Task SaveCourseSetting(CourseSetting model, string userLogin)
        {
            throw new NotImplementedException();
        }



    }
}
