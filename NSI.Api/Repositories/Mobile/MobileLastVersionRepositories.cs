using NSI.Domain.DTOs.Mobile;
using NSI.Domain.Interfaces.Mobile;
using NSI.Domain.Models.Mobile;

namespace NSI.Api.Repositories.Mobile
{
     public class MobileLastVersionRepositories 
    {
        private readonly IMobileLastVersionRepository _repo;
        public MobileLastVersionRepositories(IMobileLastVersionRepository repo)
        {
            _repo = repo;
        }
        public async Task<MobileLastVersionDto> GetVersion()
        {
            var data = await _repo.GetVersion();
            if (data == null) return null;
            return data;
        }
    }
} 
