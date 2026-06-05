using NSI.Domain.DTOs;
using NSI.Domain.Interfaces;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace NSI.Application.Services
{
    public class MenuService
    {
        private readonly IMenuRepository _repo;

        public MenuService(IMenuRepository repo)
        {
            _repo = repo;
        }

        public async Task<List<UserMenuDto>> GetMenusAsync(string userId)
        {
            return await _repo.GetMenusAsync(userId);   
        }

    }
}
