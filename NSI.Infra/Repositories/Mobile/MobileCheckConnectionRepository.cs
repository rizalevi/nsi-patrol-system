
using NSI.Domain.DTOs.Master;
using NSI.Domain.DTOs.Mobile;
using NSI.Domain.Interfaces;
using NSI.Domain.Interfaces.Master;
using NSI.Domain.Interfaces.Mobile;
using NSI.Domain.Models.Master;
using NSI.Infra.Datas;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace NSI.Infra.Repositories.Mobile
{
    public class MobileCheckConnectionRepository : IMobileCheckConnectionRepository
    {
        private readonly DbExecutor _dbExecutor;
        public MobileCheckConnectionRepository(DbExecutor dbExecutor)
        {
            _dbExecutor = dbExecutor;
        }
        public async Task<MobileCheckConnectionDto> GetData()
        {
            var result = new MobileCheckConnectionDto();

            try
            { 
                await _dbExecutor.ExecuteAsync("SELECT 1", commandType: CommandType.Text);

                result.Status = "OK";   
                result.Database = "Connected";
                result.Time = DateTime.Now;
            }
            catch (Exception ex)
            {
                result.Status = "ERROR";
                result.Database = "Failed";
                result.Time = DateTime.Now;
                result.Message = ex.Message;
            }

            return result;

        }
    }
}
 
