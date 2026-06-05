

using Dapper;
using NSI.Domain.DTOs.Inspection;
using NSI.Domain.DTOs.Mobile;
using NSI.Domain.DTOs.Tasklist;
using NSI.Domain.Interfaces.Tasklist;
using NSI.Domain.Models.Inspection;
using NSI.Domain.Models.Tasklist;
using NSI.Infra.Datas;
using NSI.Infra.Helpers;
using System.Data;

namespace NSI.Infra.Repositories.Tasklist
{
    public class NotificationRepository : INotificationRepository
    {
        private readonly DbExecutor _dbExecutor;

        public NotificationRepository(DbExecutor dbExecutor)
        {
            _dbExecutor = dbExecutor;
        }

        //public async Task<List<TaskGroupUserDto>> GetTaskGroupUser(string pUserID)
        //{
        //    string sp = "sp_WEB_TaskListRemaining_GetTaskGroupUser";
        //    var result = await _dbExecutor.QueryListAsync<TaskGroupUserDto>(sp, new { UserID = pUserID });
        //    return result.ToList();
        //}

        public async Task<List<ExportAffectedUser>> SendNotifExportData(MobileInspectionExportDto model,string userLogin)
        {
            var exportuser = model.Export_User;

            var param = new DynamicParameters();
            param.Add("@Export_User", exportuser);

            var reesult = await _dbExecutor.QueryListAsync<ExportAffectedUser>(
                "sp_WEB_ExportData_SendNotification",
                param,
                commandType: CommandType.StoredProcedure
            );

            //--------------------------------------------------------
            // RETURN AFFECTED USERS
            //--------------------------------------------------------
            return reesult.ToList();

          
        }

        public async Task SendNotifInspectorBatch(InspectionDataUploadParamDto model, string userLogin)
        {
            var param = new DynamicParameters();
            param.Add("@PlantCode", model.PlantCode);
            param.Add("@CourseCode", model.CourseCode);
            param.Add("@GroupCode", model.GroupCode);
            param.Add("@Shift", model.Shift);
            param.Add("@InspectionNo", model.InspectionNo);
            param.Add("@InspectionDate", model.InspectionDate.HasValue
                ? model.InspectionDate.Value.ToString("yyyy-MM-dd")
                : null);
            param.Add("@UserID", userLogin);

            await _dbExecutor.ExecuteAsync(
                "sp_WEB_InspectionDataInspector_SendNotification_Batch",
                param,
                commandType: CommandType.StoredProcedure,
                commandTimeout: 120
            );

            //var param = new DynamicParameters();
            //// 🔥 TVP
            //param.Add("@ApproveBatch",
            //DataTableHelper.ToDataCheckLeader(model?.ApproveBatch).AsTableValuedParameter("dbo.ApprovalBatchType"));
            //param.Add("@UserID", userLogin);

            //await _dbExecutor.ExecuteAsync(
            //    "sp_WEB_InspectionDataInspector_SendNotification_Batch",
            //    param,
            //    commandType: CommandType.StoredProcedure
            //);
        }

        public async Task SendNotifInspectorSubmitSendBack(InspectionDataUploadUpdateDto model, string userId)
        {
            string sql = @"sp_WEB_InspectionDataInspector_SendNotification_SendBack";
            await _dbExecutor.ExecuteAsync(sql, new
            {
                model.InspectionNo,
                model.PlantCode,
                model.CourseCode,
                model.GroupCode,
                model.InspectionDate,
                UserID = userId
            });

        }

        public async Task SendNotifCheckLeaderBatch(InspectionDataLeader model, string userLogin)
        {
            var param = new DynamicParameters();
            // 🔥 TVP
            param.Add("@ApproveBatch",
            DataTableHelper.ToDataCheckLeader(model?.ApproveBatch).AsTableValuedParameter("dbo.ApprovalBatchType"));
            param.Add("@UserID", userLogin);

            await _dbExecutor.ExecuteAsync(
                "sp_WEB_InspectionDataCheck_SendNotification_Batch",
                param,
                commandType: CommandType.StoredProcedure
            );
        }

        public async Task SendNotifCheckLeaderDetail(InspectionDataLeaderDetail model, string userId)
        {
            string sql = @"sp_WEB_InspectionDataCheck_SendNotification_Detail";
            await _dbExecutor.ExecuteAsync(sql, new
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

        }

        public async Task SendNotifSupervisorBatch(InspectionDataApprove model, string userLogin)
        {
            var param = new DynamicParameters();
            // 🔥 TVP
            param.Add("@ApproveBatch",
            DataTableHelper.ToDataApproval(model?.ApproveBatch).AsTableValuedParameter("dbo.ApprovalBatchType"));
            param.Add("@UserID", userLogin);

            await _dbExecutor.ExecuteAsync(
                "sp_WEB_InspectionDataApproval_SendNotification_Batch",
                param,
                commandType: CommandType.StoredProcedure
            );
        }

        public async Task SendNotifSupervisorDetail(InspectionDataApproveDetail model, string userId)
        {
            string sql = @"sp_WEB_InspectionDataApproval_SendNotification_Detail";
            await _dbExecutor.ExecuteAsync(sql, new
            {
                model.InspectionNo,
                model.CourseCode,
                model.GroupCode,
                model.InspectionDate,
                model.Frequency,
                UserID = userId
            });

        }


        //get count new notif
        public async Task<int> GetUnreadCount(string userId)
        {

            //var sql = @"
            //SELECT COUNT(1)
            //FROM dbo.NotificationRecipient
            //WHERE UserId = @UserId
            //  AND IsRead = 0
            //  AND IsDeleted = 0";
            string sql = @"sp_WEB_Notification_GetUnreadCount";
            return await _dbExecutor.QuerySingleOrDefaultAsync<int>(sql, new { UserId = userId });

        }

        /*task list per user event click badge notif*/
        public async Task<List<NotificationListDto>> GetList(string userId)
        {

            //var sql = @"
            //SELECT TOP 50
            //    nr.RecipientId,
            //    n.NotificationId,
            //    n.Title,
            //    n.Message,
            //    n.Icon,
            //    n.Url,
            //    nr.IsRead,
            //    n.CreatedDate
            //FROM dbo.NotificationRecipient nr
            //INNER JOIN dbo.Notification n 
            //    ON nr.NotificationId = n.NotificationId
            //WHERE nr.UserId = @UserId
            //  AND nr.IsDeleted = 0
            //  AND n.IsActive = 1
            //ORDER BY n.CreatedDate DESC";
            string sql = @"sp_WEB_Notification_GetList";

            var result = await _dbExecutor.QueryListAsync<NotificationListDto>(sql, new { UserId = userId }, System.Data.CommandType.StoredProcedure);

            return result.ToList();
        }

        /*UNUSED*/
        //public async Task MarkAsRead(long recipientId, string userId)
        //{
        //    var sql = @"
        //    UPDATE dbo.NotificationRecipient
        //    SET IsRead = 1,
        //        ReadDate = GETDATE()
        //    WHERE RecipientId = @RecipientId
        //      AND UserId = @UserId";

        //    await _dbExecutor.ExecuteAsync(sql, new { RecipientId = recipientId, UserId = userId }, System.Data.CommandType.Text);
        //}

        /*jika sudah diklik badge notif update sudah baca*/
        public async Task MarkAllAsRead(string userId)
        {
            //var sql = @"
            //UPDATE dbo.NotificationRecipient
            //SET IsRead = 1,
            //    ReadDate = GETDATE()
            //WHERE UserId = @UserId
            //  AND IsRead = 0";
            string sql = @"sp_WEB_Notification_MarkAllAsRead";

            await _dbExecutor.ExecuteAsync(sql, new { UserId = userId }, System.Data.CommandType.StoredProcedure);
        }

    }

       
}
