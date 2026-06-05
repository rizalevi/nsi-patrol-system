using NSI.Domain.DTOs.Mobile; 
using NSI.Domain.Interfaces.Mobile; 

namespace NSI.Api.Services.Mobile
{
    public class MobileCheckConnectionRepositories
    {
        private readonly IMobileCheckConnectionRepository _repo;
        public MobileCheckConnectionRepositories(IMobileCheckConnectionRepository repo)
        {
            _repo = repo;
        }
        public async Task<MobileCheckConnectionDto> GetData()
        {
            var data = await _repo.GetData();
            //if (data == null) return null;

            if (data == null)
            {
                return new MobileCheckConnectionDto
                {
                    Status = "ERROR",
                    Database = "Failed",
                    Time = DateTime.Now,
                    Message = "No data returned"
                };
            }

            return new MobileCheckConnectionDto
            {
                Status = data!.Status,
                Database = data.Database, 
                Time = data.Time, 
                Message = data.Message,
            };
        }
    }
}
 