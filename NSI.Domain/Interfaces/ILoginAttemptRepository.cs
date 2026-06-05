using NSI.Domain.Models.Auth;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NSI.Domain.Interfaces
{
    public interface ILoginAttemptRepository
    {
        Task<LoginAttempt?> GetByIpAsync(string ip, string userId);
        Task SaveAsync(LoginAttempt attempt);
        Task UpdateAsync(LoginAttempt attempt);
        Task ResetAsync(string ip, string userId);

    }
}
