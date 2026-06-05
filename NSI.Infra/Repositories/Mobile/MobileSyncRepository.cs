using Dapper;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using NSI.Domain.DTOs.Mobile; 
using NSI.Domain.Interfaces.Mobile;
using NSI.Domain.Models.Mobile; 
using System.Data;

namespace NSI.Infra.Repositories.Mobile
{
    public class MobileSyncRepository : IMobileSyncRepository
    {
        private readonly string _connString;
        public MobileSyncRepository(IConfiguration config)
        {
            _connString = config.GetConnectionString("DBCS");
        }

        public async Task<MobileSyncDto> GetSync(string htCode, string userLogin, DateTime? lastSync)
        { 
            using var conn = new SqlConnection(_connString);

            using var multi = await conn.QueryMultipleAsync(
                 "sp_Syncronize_Mobile",
                new
                {
                    HTCode = htCode,
                    UserLogin = userLogin,
                    LastSync = lastSync
                },
                commandType: CommandType.StoredProcedure
            );

            return new MobileSyncDto
            {
                // ⚠️ urutan HARUS sama dengan SP
                Users = (await multi.ReadAsync<MobileUserSetupModel>()).ToList(),  
                InspectionPointsSum = (await multi.ReadAsync<MobileInspectionPointSumModel>()).ToList(),
                MstCourse = (await multi.ReadAsync<MobileMstCourseModel>()).ToList(),
                InspectionPoints = (await multi.ReadAsync<MobileInspectionPointModel>()).ToList(),
                MstUnit = (await multi.ReadAsync<MobileMstUnitModel>()).ToList(),
                HTDetail = (await multi.ReadAsync<MobileHTDetailModel>()).ToList(),
                MstGroup = (await multi.ReadAsync<MobileMstGroupModel>()).ToList(),
                GroupCourse = (await multi.ReadAsync<MobileGroupCourseModel>()).ToList(),
                MstHT = (await multi.ReadAsync<MobileMstHTModel>()).ToList(),
                GroupSetup = (await multi.ReadAsync<MobileGroupSetupModel>()).ToList(),
                MstMobile = (await multi.ReadAsync<MobileMstMobileModel>()).ToList(),
            };
        } 
    }
}
