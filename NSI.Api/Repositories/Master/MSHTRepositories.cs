using ClosedXML;
using NSI.Domain.DTOs.Master;
using NSI.Domain.Interfaces;
using NSI.Domain.Interfaces.Master;
using NSI.Domain.Models.Master;

namespace NSI.Api.Services.Master
{
    public class MSHTRepositories
    {
        private readonly IMSDeviceRepository _repo;

        public MSHTRepositories(IMSDeviceRepository repo)
        {
            _repo = repo;
        }

        #region MSHT
        public async Task<IEnumerable<MSDeviceDto>> GetList()
        {
            var data = await _repo.GetList();
            return data.Select(x => new MSDeviceDto
            {
                HT_Code = x.HT_Code,
                HT_Name = x.HT_Name,
                Description = x.Description,
                Plant_Code = x.Plant_Code,
                Plant_Name = x.Plant_Name,
                Serial_No = x.Serial_No,
                Register_Date = x.Register_Date,
                Register_User = x.Register_User,
                Last_Update = x.Last_Update,
                Last_User = x.Last_User
            });
        }

        public async Task<MSDeviceDto> GetData(string hTCode)
        {
            var x = await _repo.GetData(hTCode);
            if (x == null) return null;

            return new MSDeviceDto
            {
                HT_Code = x.HT_Code,
                HT_Name = x.HT_Name,
                Description = x.Description,
                Plant_Code = x.Plant_Code,
                Plant_Name = x.Plant_Name,
                Serial_No = x.Serial_No,
                Register_Date = x.Register_Date,
                Register_User = x.Register_User,
                Last_Update = x.Last_Update,
                Last_User = x.Last_User
            };
        }

        public async Task Create(MSDeviceDto x, string userId)
        {
            var entity = new MSDeviceDto
            {
                HT_Code = x.HT_Code,
                HT_Name = x.HT_Name,
                Description = x.Description,
                Plant_Code = x.Plant_Code,
                Plant_Name = x.Plant_Name,
                Serial_No = x.Serial_No,
                Register_Date = x.Register_Date,
                Register_User = x.Register_User,
                Last_Update = x.Last_Update,
                Last_User = x.Last_User
            };

            await _repo.Create(entity, userId);
        }

        public async Task Update(MSDeviceDto x, string userId)
        {
            var entity = new MSDeviceDto
            {
                HT_Code = x.HT_Code,
                HT_Name = x.HT_Name,
                Description = x.Description,
                Plant_Code = x.Plant_Code,
                Plant_Name = x.Plant_Name,
                Serial_No = x.Serial_No,
                Register_Date = x.Register_Date,
                Register_User = x.Register_User,
                Last_Update = x.Last_Update,
                Last_User = x.Last_User
            };

            await _repo.Update(entity, userId);
        }

        public async Task Remove(string hTCode, string userID)
        {
            await _repo.Remove(hTCode, userID);
        }

        public async Task<IEnumerable<MSDeviceDto>> GetPlants()
        {
            return await _repo.GetPlants();
        }
        #endregion

        #region MSHTDetail
        public async Task<IEnumerable<MSHTDetailDto>> GetListHTDetail(string hTCode, string plantCode)
        {
            var data = await _repo.GetListHTDetail(hTCode, plantCode);
            return data.Select(x => new MSHTDetailDto
            {
                HT_Code = x.HT_Code,
                HT_Name = x.HT_Name,
                Description = x.Description,
                Plant_Code = x.Plant_Code,
                Plant_Name = x.Plant_Name,
                Serial_No = x.Serial_No,
                Register_Date = x.Register_Date,
                Register_User = x.Register_User,
                Last_Update = x.Last_Update,
                Last_User = x.Last_User,
                Check = x.Check,
                Course_Code = x.Course_Code,
                Title = x.Title,
                Doc_No = x.Doc_No,
                Check_Type = x.Check_Type,
                Shift_Cls = x.Shift_Cls
            });
        }
         

        public async Task CreateHTDetail(MSHTDetailDto x, string userId)
        {
            var entity = new MSHTDetailDto
            {
                HT_Code = x.HT_Code, 
                Plant_Code = x.Plant_Code, 
                Course_Code = x.Course_Code,
                Last_User = x.Last_User,
                Check = x.Check
            };

            await _repo.CreateHTDetail(entity, userId);
        }
          
        public async Task RemoveHTDetail(string hTCode, string plantCode, string courseCode, string userID)
        {
            await _repo.RemoveHTDetail(hTCode, plantCode, courseCode, userID);
        }

        public async Task<IEnumerable<MSHTDetailDto>> GetPlantsHTDetail()
        {
            return await _repo.GetPlantsHTDetail();
        }

        public async Task<IEnumerable<MSHTDetailDto>> GetHTCodesHTDetail()
        {
            return await _repo.GetHTCodesHTDetail();
        }
        #endregion
    }
}
