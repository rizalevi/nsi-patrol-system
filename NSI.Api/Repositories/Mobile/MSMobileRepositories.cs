using NSI.Domain.DTOs.Master;
using NSI.Domain.DTOs.Mobile;
using NSI.Domain.Interfaces;
using NSI.Domain.Interfaces.Master;
using NSI.Domain.Models.Master;
using NSI.Domain.Models.Mobile;


namespace NSI.Api.Services.Master
{ 
    public class MSMobileRepositories
    {
        private readonly IMSMobileRepository _repo;

        public MSMobileRepositories(IMSMobileRepository repo)
        {
            _repo = repo;
        }

        public async Task<IEnumerable<MSMobileDto>> GetAll(DateTime? lastSync)
        {
            var data = await _repo.GetAll(lastSync);
            return data.Select(x => new MSMobileDto
            {
                Mobile_Code = x.Mobile_Code,
                Mobile_Name = x.Mobile_Name,
                Plant_Code = x.Plant_Code,
                Register_Date = x.Register_Date,
                Register_User = x.Register_User,
                Last_Update = x.Last_Update,
                Last_User = x.Last_User,
                Mobile_ID = x.Mobile_ID
            });
        }  

        public async Task Upset(MSMobile model)
        {
            var entity = new MSMobile
            {
                MobileCode = model.MobileCode,
                MobileName = model.MobileName,
                MobileID = model.MobileID,
                UserID = model.UserID,
            };

            await _repo.Upset(entity);
        }
    }
}
