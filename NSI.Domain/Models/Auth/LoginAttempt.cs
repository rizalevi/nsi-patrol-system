
namespace NSI.Domain.Models.Auth
{
    public class LoginAttempt
    {
        public Guid Id { get; private set; } = Guid.NewGuid();
        public string IpAddress { get; private set; }
        public string UserId { get; private set; }
        public int FailedCount { get; private set; }
        public DateTime? LockedUntil { get; private set; }
        public DateTime? LastAttemptAt { get; private set; }
        private static readonly TimeSpan MIN_INTERVAL = TimeSpan.FromMinutes(5);


        // ✅ WAJIB untuk Dapper
        public LoginAttempt() { }

        public LoginAttempt(string ip, string userId)
        {
            IpAddress = ip;
            UserId = userId;
            FailedCount = 0;

        }
        // (optional) constructor bisnis kamu
        public LoginAttempt(
            Guid id,
            string ipAddress,
            string userId,
            int failedCount,
            DateTime? lockedUntil,
            DateTime lastAttemptAt)
        {
            Id = id;
            IpAddress = ipAddress;
            UserId = userId;
            FailedCount = failedCount;
            LockedUntil = lockedUntil;
            LastAttemptAt = lastAttemptAt;
        }

        public void RegisterFailure(int maxAttempt, TimeSpan lockDuration)
        {
            EnforceMinInterval();

            FailedCount++;
            LastAttemptAt = DateTime.UtcNow;

            if (FailedCount >= maxAttempt)
            {
                LockedUntil = DateTime.UtcNow.Add(lockDuration);
            }
        }
        public void RegisterSuccess()
        {
            FailedCount = 0;
            LockedUntil = null;
            LastAttemptAt = DateTime.UtcNow;
        }

        private void EnforceMinInterval()
        {
            if (LastAttemptAt.HasValue &&
                DateTime.UtcNow - LastAttemptAt < MIN_INTERVAL)
            {
                throw new InvalidOperationException(
                    $"Too many attempts in short time. Try again later.");
            }
        }

        public bool IsLocked()
            => LockedUntil.HasValue && LockedUntil > DateTime.UtcNow;

    }
}
