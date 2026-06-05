using Dapper;
using NSI.Domain.DTOs.Inspection;
using NSI.Domain.Interfaces.Inspection;
using NSI.Domain.Models.Inspection;
using NSI.Domain.Models.Tasklist;
using NSI.Infra.Datas;
using NSI.Infra.Helpers;
using System.Data;

namespace NSI.Infra.Repositories.Inspection
{
    public class InspectionDataLeaderRepository : IInspectionDataLeaderRepository
    {
        private readonly DbExecutor _dbExecutor;
       
        public InspectionDataLeaderRepository(DbExecutor dbExecutor)
        {
            _dbExecutor = dbExecutor;
        }

        public async Task<List<InspectionDataLeaderListDto>> GetList(InspectionDataLeaderParamDto model)
        {

            string sp = "sp_WEB_InspectionData_CheckLeader_List";
            var result = await _dbExecutor.QueryListAsync<InspectionDataLeaderListDto>(sp, model);
            return result.ToList();

            
        }
        public async Task<List<InspectionDataLeaderListDetailDto>> GetDetailInspection(LeaderParamDetailDto model)
        {
            string sp = "sp_WEB_InspectionData_CheckLeader_ListDetail";
            var result =  await _dbExecutor.QueryListAsync<InspectionDataLeaderListDetailDto>(sp, model);
            return result.ToList();
        }

        public async Task<ResponseApprove> ApproveBatch(InspectionDataLeader model, string userLogin)
        {
            var param = new DynamicParameters();
            // 🔥 TVP
            param.Add("@ApproveBatch",
            DataTableHelper.ToDataCheckLeader(model?.ApproveBatch).AsTableValuedParameter("dbo.LeaderBatchType"));
            param.Add("@UserID", userLogin);

           var result= await _dbExecutor.ExecuteAsyncApproval(
                "sp_WEB_InspectionData_CheckLeader_ApproveBatch",
                param,
                commandType: CommandType.StoredProcedure,
                commandTimeout: 120
            );


            return result ?? new ResponseApprove();
        }

        public async Task<ResponseApprove> ApproveCheckDetail(InspectionDataLeaderDetail model, string userId)
        {
            string sql = @"sp_WEB_InspectionData_CheckLeader_ApproveDetail";
            var result =  await _dbExecutor.ExecuteAsyncApproval(sql, new
            {
                model.InspectionNo,
                model.CourseCode,
                model.GroupCode,
                model.InspectionDate,
                model.Frequency,
                model?.Remarks,
                model?.RemarksSendBack,
                model?.Type,
                UserID = userId
            });
            return result ?? new ResponseApprove();
        }

       
    }
}
