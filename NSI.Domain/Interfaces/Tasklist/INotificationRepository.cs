using NSI.Domain.DTOs.Inspection;
using NSI.Domain.DTOs.Mobile;
using NSI.Domain.DTOs.Tasklist;
using NSI.Domain.Models.Inspection;
using NSI.Domain.Models.Tasklist;

namespace NSI.Domain.Interfaces.Tasklist
{
    public interface INotificationRepository
    {
        Task<List<ExportAffectedUser>> SendNotifExportData(MobileInspectionExportDto model, string userLogin);

        Task SendNotifInspectorBatch(InspectionDataUploadParamDto model, string userLogin);

        Task SendNotifInspectorSubmitSendBack(InspectionDataUploadUpdateDto model, string userLogin);

        Task SendNotifCheckLeaderBatch(InspectionDataLeader model, string userLogin);

        Task SendNotifCheckLeaderDetail(InspectionDataLeaderDetail model, string userLogin);

        Task SendNotifSupervisorBatch(InspectionDataApprove model, string userLogin);

        Task SendNotifSupervisorDetail(InspectionDataApproveDetail model, string userLogin);


        Task<int> GetUnreadCount(string userId);

        Task<List<NotificationListDto>> GetList(string userId);

        //Task MarkAsRead(long recipientId, string userId);

        Task MarkAllAsRead(string userId);
    }
        
    //public interface INotificationRepository
    //{
    //    Task<long> CreateNotification(NotificationDto model, List<string> userIds);

    //    Task<List<NotificationListDto>> GetUserNotifications(string userId);

    //    Task<int> GetUnreadCount(string userId);

    //    Task MarkAsRead(long recipientId, string userId);

    //    Task MarkAllAsRead(string userId);

    //    Task ArchiveOldNotifications();
    //}
}
