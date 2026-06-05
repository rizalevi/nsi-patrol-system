using NSI.Domain.DTOs.Security;
using NSI.Domain.Interfaces.Security;
using NSI.Domain.Models.Security;
using NSI.Infra.Datas;

namespace NSI.Infra.Repositories.Security
{
    public class PermissionUserRepository : IPermissionUserRepository
    {
        private readonly DbExecutor _dbExecutor;

        public PermissionUserRepository(DbExecutor dbExecutor)
        {
            _dbExecutor = dbExecutor;
        }

        public async Task<PermissionDto?> GetPermission(PermissionParamDto payload)
        {

            string sp = "sp_Web_SS_User_GetPermission";

            var model = await _dbExecutor.QueryFirstOrDefaultAsync<PermissionDto>(
                sp,payload
            );

            return model;

        }
    }
}
