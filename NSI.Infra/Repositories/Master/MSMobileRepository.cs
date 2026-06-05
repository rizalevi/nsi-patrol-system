using NSI.Domain.DTOs.Master;
using NSI.Domain.Interfaces;
using NSI.Domain.Interfaces.Master;
using NSI.Domain.Models.Master;
using NSI.Infra.Datas;


namespace NSI.Infra.Repositories.Master
{
    public class MSMobileRepository : IMSMobileRepository
    {
        private readonly DbExecutor _dbExecutor;

        public MSMobileRepository(DbExecutor dbExecutor)
        {
            _dbExecutor = dbExecutor;
        }

        public async Task<List<MSMobileDto>> GetAll(DateTime? lastSync)
        {
            string sp = "sp_Web_MSMobile_List";
            return (await _dbExecutor.QueryListAsync<MSMobileDto>(sp, new { LastSync = lastSync })).ToList();
        }

        public async Task Upset(MSMobile model)
        { 
            string sql = @"sp_Web_MSMobile_Upset";
            await _dbExecutor.ExecuteAsync(sql, new
            {
                model.MobileCode,
                model.MobileName,
                model.MobileID,
                model.UserID, 
            });
        }
    }
}
