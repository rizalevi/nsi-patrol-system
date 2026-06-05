
namespace NSI.Domain.DTOs.Tasklist
{
    public class NotificationDto
    {
        public string? Title { get; set; }
        public string? Message { get; set; }

        public string? NotificationType { get; set; }
        public string? ReferenceType { get; set; }
        public string? ReferenceId { get; set; }

        public string? Url { get; set; }
        public string? Icon { get; set; }
        public int? UnreadCount { get; set; } // optional helper
    }

    public class NotificationRecipientDto
    {
        public long RecipientId { get; set; }
        public long NotificationId { get; set; }
        public string? UserId { get; set; }

        public bool? IsRead { get; set; }
        public DateTime? ReadDate { get; set; }

        public bool? IsDeleted { get; set; }
        public bool? IsDelivered { get; set; }

        public DateTime? DeliveredDate { get; set; }
    }

    public class NotificationListDto
    {
        public long? NotificationId { get; set; }
        public long? RecipientId { get; set; }

        public string? Title { get; set; }
        public string? Message { get; set; }

        public string? Icon { get; set; }
        public string? Url { get; set; }

        public bool? IsRead { get; set; }
        public DateTime? CreatedDate { get; set; }
    }


    public class NotificationCountDto
    {
        public int? Count { get; set; }
    }
}
