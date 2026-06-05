using NSI.Domain.DTOs.Master;
using NSI.Domain.Interfaces.Master;
using NSI.Domain.Models.Master;
using NSI.Infra.Datas;

namespace NSI.Infra.Repositories.Master
{
    public class MSUnitClassificationRepository : IMSUnitClassificationRepository
    {
        private readonly DbExecutor _dbExecutor;

        public MSUnitClassificationRepository(DbExecutor dbExecutor)
        {
            _dbExecutor = dbExecutor;
        }

        public async Task<List<MSUnitClassificationDto>> GetAll()
        {
            // Menggunakan SP GetData karena SP ini me-return seluruh data (tanpa parameter filter)
            string sp = "sp_Web_MSUnitClassification_GetData";
            return (await _dbExecutor.QueryListAsync<MSUnitClassificationDto>(sp)).ToList();
        }

        public async Task<MSUnitClassification> GetData(string unitCode)
        {
            // Catatan: Stored Procedure untuk mengambil 1 data spesifik (berdasarkan UnitCode) 
            // tidak disertakan di pertanyaan. Ini adalah placeholder.
            string sp = "sp_Web_MSUnitClassification_GetById";
            return await _dbExecutor.QueryFirstOrDefaultAsync<MSUnitClassification>(sp, new { UnitCode = unitCode });
        }

        public async Task Create(MSUnitClassification model, string userId)
        {
            string sql = "sp_Web_MSUnitClassification_Insert";
            await _dbExecutor.ExecuteAsync(sql, new
            {
                UnitCode = model.UnitCode,
                UnitDesc = model.UnitDescription, // Sesuaikan dengan properti di class model Anda
                MethodCode = model.MethodCode,
                MethodDescription = model.MethodDescription,
                Option1 = model.Option1,
                Option2 = model.Option2,
                Option3 = model.Option3,
                RegisterUser = userId,
                LastUser = userId
            });
        }

        public async Task Update(MSUnitClassification model, string userId)
        {
            // Catatan: Stored Procedure untuk Update tidak disertakan di pertanyaan.
            // Anda perlu membuat SP-nya di SQL dan menyesuaikan parameter di bawah ini.
            string sql = "sp_Web_MSUnitClassification_Update";
            await _dbExecutor.ExecuteAsync(sql, new
            {
                UnitCode = model.UnitCode,
                UnitDesc = model.UnitDescription,
                MethodCode = model.MethodCode,
                MethodDescription = model.MethodDescription,
                Option1 = model.Option1,
                Option2 = model.Option2,
                Option3 = model.Option3,
                LastUser = userId
            });
        }

        public async Task Remove(string unitCode, string userId, string ipAddress)
        {
            string sql = "sp_Web_MSUnitClassification_Delete";
            // Parameter disesuaikan dengan SP. 
            // ipAddress di-skip karena SP sudah memanggil "EXEC sp_get_ip_address @IPAddress OUT"
            await _dbExecutor.ExecuteAsync(sql, new
            {
                Unit_Code = unitCode,
                UserID = userId
            });
        }
    }
}