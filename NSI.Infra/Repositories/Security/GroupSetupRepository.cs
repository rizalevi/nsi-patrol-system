using Dapper;
using NSI.Domain.DTOs.Security;
using NSI.Domain.Interfaces.Security;
using NSI.Infra.Datas;
using System.Data;
using NSI.Infra.Helpers;

namespace NSI.Infra.Repositories.Security
{
    public class GroupSetupRepository : IGroupSetupRepository
    {
        private readonly DbExecutor _dbExecutor;
       
        public GroupSetupRepository(DbExecutor dbExecutor)
        {
            _dbExecutor = dbExecutor;
        }


        public async Task<List<GroupSetupDDLDto>> GetGroupDDL()
        {
            string sp = "sp_Web_SS_GroupSetup_DDL";
            var result = await _dbExecutor.QueryListAsync<GroupSetupDDLDto>(sp);
            return result.ToList();
        }

        public async Task<List<GroupSetupDto>> GetAll()
        {
            string sp = "sp_Web_SS_GroupSetup_List";
            var result = await _dbExecutor.QueryListAsync<GroupSetupDto>(sp);
            return result.ToList();
        }

        
        public async Task<GroupSetup> GetData(string groupId)
        {
            string sp = "sp_Web_SS_GroupSetup_GetData";
            return await _dbExecutor.QueryFirstOrDefaultAsync<GroupSetup>(sp, new { GroupID = groupId });
        }


        public async Task Save(GroupSetup model, string userLogin)
        {
            var param = new DynamicParameters();

            param.Add("@GroupID", model.GroupID);
            param.Add("@GroupName", model.GroupName);
            param.Add("@ClusterCode", model.ClusterCode);
            param.Add("@PlantCode", model.PlantCode);

            param.Add("@InspectorApproval", model.InspectorApproval);
            param.Add("@LeaderApproval", model.LeaderApproval);
            param.Add("@CheckerApproval", model.CheckerApproval);
            param.Add("@SupervisorApproval", model.SupervisorApproval);

            // 🔥 TVP
            param.Add("@DetailPrivilege",
            DataTableHelper.ToPrivilegeTable(model.DetailPrivilege)
            .AsTableValuedParameter("dbo.GroupPrivilegeType"));

            param.Add("@DetailCourse",
                DataTableHelper.ToCourseTable(model.DetailCourse)
                .AsTableValuedParameter("dbo.GroupCourseType"));

            param.Add("@UserID", userLogin);

            await _dbExecutor.ExecuteAsync(
                "sp_Web_SS_GroupSetup_Save",
                param,
                commandType: CommandType.StoredProcedure,
                commandTimeout: 120
            );
        }
        public async Task Remove(string groupId, string userLogin, string ipAddress)
        {
            string sql = "sp_Web_SS_GroupSetup_Delete";
            await _dbExecutor.ExecuteAsync(sql, new { GroupID = groupId, UserID = userLogin, IPAddress = ipAddress });
        }

       
    }
}
