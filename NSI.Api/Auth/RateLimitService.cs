using DocumentFormat.OpenXml.Spreadsheet;
using NSI.Domain.Interfaces;
using NSI.Domain.Models.Auth;

namespace NSI.Api.Auth
{
    public class RateLimitService
    {
        private const int MAX_ATTEMPT = 5;
        private static readonly TimeSpan LOCK_DURATION = TimeSpan.FromMinutes(5);

        private readonly ILoginAttemptRepository _repo;

        public RateLimitService(ILoginAttemptRepository repo)
        {
            _repo = repo;
        }

        public async Task CheckAsync(string ip,string userid)
        {
            var attempt = await _repo.GetByIpAsync(ip, userid );

            if (attempt == null) return;
            
            if (attempt.IsLocked())
                throw new UnauthorizedAccessException(
                    $"IP locked until {attempt.LockedUntil:HH:mm:ss}");

            // lock sudah lewat -> reset
            if (attempt.LockedUntil.HasValue &&
                attempt.LockedUntil <= DateTime.UtcNow)
            {
                await _repo.ResetAsync(ip, userid);
            }
        
        }

        public async Task RegisterFailureAsync(string ip, string userid)
        {
            var attempt = await _repo.GetByIpAsync(ip, userid);

            try
            {
                if (attempt == null)
                {
                    attempt = new LoginAttempt(ip, userid);
                    attempt.RegisterFailure(MAX_ATTEMPT, LOCK_DURATION);
                    await _repo.SaveAsync(attempt);
                }
                else
                {
                    attempt.RegisterFailure(MAX_ATTEMPT, LOCK_DURATION);
                    await _repo.UpdateAsync(attempt);
                }
            }
            catch (InvalidOperationException ex)
            {
                // slow brute-force detected
                throw new TooManyRequestsException(ex.Message);
            }
        }

        public async Task ResetAsync(string ip, string userid)
        {
            await _repo.ResetAsync(ip, userid);
            //var attempt = await _repo.GetByIpAsync(ip, userid);
            //if (attempt == null) return;

            //attempt.RegisterSuccess();
            //await _repo.UpdateAsync(attempt);
        }
    }

    public class TooManyRequestsException : Exception
    {
        public TooManyRequestsException(string message) : base(message) { }
    }
}
