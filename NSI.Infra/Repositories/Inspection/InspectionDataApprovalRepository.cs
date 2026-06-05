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
    public class InspectionDataApprovalRepository : IInspectionDataApprovalRepository
    {
        private readonly DbExecutor _dbExecutor;
       
        public InspectionDataApprovalRepository(DbExecutor dbExecutor)
        {
            _dbExecutor = dbExecutor;
        }

        public async Task<List<InspectionDataApprovalListDto>> GetList(InspectionDataApprovalParamDto model)
        {

            string sp = "sp_WEB_InspectionData_Approval_List";
            var result = await _dbExecutor.QueryListAsync<InspectionDataApprovalListDto>(sp, model);
            return result.ToList();

            
        }
        public async Task<List<InspectionDataApprovalListDetailDto>> GetDetailInspection(ApprovalParamDetailDto model)
        {
            string sp = "sp_WEB_InspectionData_Approval_ListDetail";
            var result =  await _dbExecutor.QueryListAsync<InspectionDataApprovalListDetailDto>(sp, model);
            return result.ToList();
        }

        public async Task<ResponseApprove> ApproveBatch(InspectionDataApprove model, string userLogin)
        {
            var param = new DynamicParameters();
            // 🔥 TVP
            param.Add("@ApproveBatch",
            DataTableHelper.ToDataApproval(model?.ApproveBatch).AsTableValuedParameter("dbo.ApprovalBatchType"));
            param.Add("@UserID", userLogin);

            var result = await _dbExecutor.ExecuteAsyncApproval(
                "sp_WEB_InspectionData_Approval_ApproveBatch",
                param,
                commandType: CommandType.StoredProcedure,
                commandTimeout: 120
            );

            return result ?? new ResponseApprove();
        }

        public async Task<ResponseApprove> ApproveDetail(InspectionDataApproveDetail model, string userId)
        {
            string sql = @"sp_WEB_InspectionData_Approval_ApproveDetail";
            var result=await _dbExecutor.ExecuteAsyncApproval(sql, new
            {
                model.InspectionNo,
                model.CourseCode,
                model?.GroupCode,
                model?.InspectionDate,
                model?.Frequency,
                UserID = userId
            });
            return result ?? new ResponseApprove();
        }

        //public async Task ApproveBatchPushNotification(InspectionDataApprove model, string userLogin)
        //{
        //    var param = new DynamicParameters();
        //    // 🔥 TVP
        //    param.Add("@ApproveBatch",
        //    DataTableHelper.ToDataApproval(model?.ApproveBatch).AsTableValuedParameter("dbo.ApprovalBatchType"));
        //    param.Add("@UserID", userLogin);

        //    await _dbExecutor.ExecuteAsync(
        //        "sp_WEB_InspectionData_Approval_ApproveBatch",
        //        param,
        //        commandType: CommandType.StoredProcedure,
        //        commandTimeout: 120
        //    );
        //}


    }
}
