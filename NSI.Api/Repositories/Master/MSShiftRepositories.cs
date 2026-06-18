using ClosedXML;
using NSI.Domain.DTOs.Master;
using NSI.Domain.Interfaces;
using NSI.Domain.Interfaces.Master;
using NSI.Domain.Models.Master;

namespace NSI.Api.Services.Master
{
    public class MSShiftRepositories
    {
        private readonly IMSShiftRepository _repo;

        public MSShiftRepositories(IMSShiftRepository repo)
        {
            _repo = repo;
        }

        public async Task<IEnumerable<MSShiftDto>> GetList()
        {
            var data = await _repo.GetList();
            return data.Select(x => new MSShiftDto
            {
                Shift_Cls = x.Shift_Cls,
                Inspection_Time = x.Inspection_Time,
                Register_Date = x.Register_Date,
                Register_User = x.Register_User,
                Last_Update = x.Last_Update,
                Last_User = x.Last_User
            });
        } 

        public async Task<MSShiftDto> GetData(string shiftCls)
        {
            var data = await _repo.GetData(shiftCls);
            if (data == null) return null;

            return new MSShiftDto
            {
                Shift_Cls = data.Shift_Cls,
                Inspection_Time = data.Inspection_Time,
                Register_Date = data.Register_Date,
                Register_User = data.Register_User,
                Last_Update = data.Last_Update,
                Last_User = data.Last_User
            };
        }

        public async Task Create(MSShiftDto model, string userId)
        {
            var entity = new MSShiftDto
            {
                Shift_Cls = model.Shift_Cls,
                Inspection_Time = model.Inspection_Time,
                Register_Date = model.Register_Date,
                Register_User = model.Register_User,
                Last_Update = model.Last_Update,
                Last_User = model.Last_User
            };

            await _repo.Create(entity, userId);
        }

        public async Task Update(MSShiftDto model, string userId)
        {
            var entity = new MSShiftDto
            {
                Shift_Cls = model.Shift_Cls,
                Inspection_Time = model.Inspection_Time,
                Register_Date = model.Register_Date,
                Register_User = model.Register_User,
                Last_Update = model.Last_Update,
                Last_User = model.Last_User
            };

            await _repo.Update(entity, userId);
        }

        public async Task Remove(string shiftCls, string userID, string ipAddress)
        {
            await _repo.Remove(shiftCls, userID,ipAddress);
        }
    }
}
