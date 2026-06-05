using NSI.Domain.DTOs.Master;
using NSI.Domain.Interfaces.Master;
using NSI.Domain.Models.Master;

namespace NSI.Api.Services.Master
{
    // Mengikuti standar penamaan dari contoh Anda
    public class MSUnitClassificationRepositories(IMSUnitClassificationRepository repo)
    {
        public async Task<IEnumerable<MSUnitClassificationDto>> GetAll()
        {
            //var data = await repo.GetAll();

            //return data.Select(x => new MSUnitClassificationDto
            //{
            //    UnitCode = x.UnitCode?.Trim(),               // Tambahkan Trim()
            //    UnitDescription = x.UnitDescription?.Trim(), // Tambahkan Trim()
            //    MethodCode = x.MethodCode?.Trim(),
            //    MethodDescription = x.MethodDescription?.Trim(),
            //    Option1 = x.Option1?.Trim(),
            //    Option2 = x.Option2?.Trim(),
            //    Option3 = x.Option3?.Trim(),
            //    RegisterDate = x.RegisterDate,
            //    RegisterUser = x.RegisterUser?.Trim(),
            //    LastUpdate = x.LastUpdate,
            //    LastUser = x.LastUser?.Trim()
            //});
            //buat lebih simpel dan aman pakai trim
            return await repo.GetAll();
        }

        public async Task<MSUnitClassification> GetData(string unitCode)
        {
            // Tambahkan Trim() pada parameter sebelum dilempar ke Repo / Database
            var cleanCode = unitCode?.Trim();
            var data = await repo.GetData(cleanCode);

            if (data == null) return null;

            return new MSUnitClassification
            {
                UnitCode = data.UnitCode?.Trim(),               // Tambahkan Trim()
                UnitDescription = data.UnitDescription?.Trim(), // Tambahkan Trim()
                MethodCode = data.MethodCode?.Trim(),
                MethodDescription = data.MethodDescription?.Trim(),
                Option1 = data.Option1?.Trim(),
                Option2 = data.Option2?.Trim(),
                Option3 = data.Option3?.Trim()
            };
        }

        public async Task Create(MSUnitClassification model, string userId)
        {
            // Memastikan hanya data yang diperlukan untuk Insert yang diteruskan ke Repo
            //var entity = new MSUnitClassification
            //{
            //    UnitCode = model.UnitCode,
            //    UnitDescription = model.UnitDescription,
            //    MethodCode = model.MethodCode,
            //    MethodDescription = model.MethodDescription,
            //    Option1 = model.Option1,
            //    Option2 = model.Option2,
            //    Option3 = model.Option3
            //};

            //await repo.Create(entity, userId);

            // buat lebih simpel
            await repo.Create(model, userId);
        }

        public async Task Update(MSUnitClassification model, string userId)
        {
            // Memastikan hanya data yang diperlukan untuk Update yang diteruskan ke Repo
            //var entity = new MSUnitClassification
            //{
            //    UnitCode = model.UnitCode,
            //    UnitDescription = model.UnitDescription,
            //    MethodCode = model.MethodCode,
            //    MethodDescription = model.MethodDescription,
            //    Option1 = model.Option1,
            //    Option2 = model.Option2,
            //    Option3 = model.Option3
            //};

            //await repo.Update(entity, userId);
            // buat lebih simpel
            await repo.Update(model, userId);
        }

        public async Task Remove(string unitCode, string userId, string ipAddress)
        {
            await repo.Remove(unitCode, userId, ipAddress);
        }
    }
}