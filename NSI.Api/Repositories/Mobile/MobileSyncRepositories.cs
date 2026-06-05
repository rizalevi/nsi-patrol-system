using NSI.Domain.DTOs.Mobile;
using NSI.Domain.Interfaces.Mobile;
using NSI.Domain.Models.Mobile;

namespace NSI.Api.Repositories.Mobile
{
    public class MobileSyncRepositories
    {
        private readonly IMobileSyncRepository _repo;
        public MobileSyncRepositories(IMobileSyncRepository repo)
        {
            _repo = repo;
        }
        public async Task<MobileSyncDto> GetSync(string htCode, string userLogin, DateTime? lastSync)
        {
            var data = await _repo.GetSync(htCode, userLogin, lastSync);
            //if (data == null) return null;

            if (data == null)
            {
                return new MobileSyncDto
                {
                    Users = new List<MobileUserSetupModel>(),
                    InspectionPointsSum = new List<MobileInspectionPointSumModel>(),
                    MstCourse = new List<MobileMstCourseModel>(),
                    InspectionPoints = new List<MobileInspectionPointModel>(),
                    MstUnit = new List<MobileMstUnitModel>(),
                    HTDetail = new List<MobileHTDetailModel>(),
                    MstGroup = new List<MobileMstGroupModel>(),
                    GroupCourse = new List<MobileGroupCourseModel>(),
                    MstHT = new List<MobileMstHTModel>(),
                    GroupSetup = new List<MobileGroupSetupModel>(),
                    MstMobile = new List<MobileMstMobileModel>(),
                };
            }

            return data;
        }
    }
}
