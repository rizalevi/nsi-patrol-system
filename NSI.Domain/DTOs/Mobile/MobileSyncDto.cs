using NSI.Domain.Models.Mobile; 

namespace NSI.Domain.DTOs.Mobile
{
    public class MobileSyncDto
    { 
        public List<MobileUserSetupModel> Users { get; set; }
        public List<MobileInspectionPointSumModel> InspectionPointsSum { get; set; }
        public List<MobileMstCourseModel> MstCourse { get; set; }
        public List<MobileInspectionPointModel> InspectionPoints { get; set; }
        public List<MobileMstUnitModel> MstUnit { get; set; }
        public List<MobileHTDetailModel> HTDetail { get; set; }
        public List<MobileMstGroupModel> MstGroup { get; set; }
        public List<MobileGroupCourseModel> GroupCourse { get; set; }
        public List<MobileMstHTModel> MstHT { get; set; }
        public List<MobileGroupSetupModel> GroupSetup { get; set; }
        public List<MobileMstMobileModel> MstMobile { get; set; }
    }
}
