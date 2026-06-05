using NSI.Domain.DTOs;
using System.Collections.Generic;

namespace NSI.Domain.Interfaces
{
    public interface IMenuRepository
    {
        Task<List<UserMenuDto>> GetMenusAsync(string userId);
    }
}
