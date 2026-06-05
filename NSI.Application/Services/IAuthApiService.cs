using NSI.Domain.Models.Auth;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NSI.Application.Services
{
    public interface IAuthApiService
    {
        Task<LoginResponse?> LoginAsync(string UserId, string Password);
    }
}
