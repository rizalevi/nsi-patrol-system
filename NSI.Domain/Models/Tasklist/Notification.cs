namespace NSI.Domain.Models.Tasklist
{
    public class ExportAffectedUser
    {

        public string? UserId { get; set; }
    }

    public class ResponseApprove
    {
        public bool NeedPushNotification { get; set; }

        public string? TargetUser { get; set; } 
    }

}
