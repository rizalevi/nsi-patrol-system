using NSI.Domain.DTOs.Mobile;
using NSI.Domain.Models.Inspection;
using NSI.Domain.Models.Master;
using NSI.Domain.Models.Security;
using System.Data;
namespace NSI.Infra.Helpers
{
    public static class DataTableHelper
    {
        public static DataTable ToPrivilegeTable(List<GroupPrivilege> list)
        {
            var dt = new DataTable();
            dt.Columns.Add("MenuID", typeof(string));
            dt.Columns.Add("AllowAccess", typeof(bool));
            dt.Columns.Add("AllowUpdate", typeof(bool));
            dt.Columns.Add("AllowDelete", typeof(bool));

            foreach (var item in list)
            {
                dt.Rows.Add(item.MenuID, item.AllowAccess, item.AllowUpdate, item.AllowDelete);
            }

            return dt;
        }

        public static DataTable ToCourseTable(List<CourseSetting> list)
        {
            var dt = new DataTable();
            dt.Columns.Add("CourseCode", typeof(string));
            dt.Columns.Add("Show", typeof(bool));

            foreach (var item in list)
            {
                dt.Rows.Add(item.CourseCode, item.Show);
            }

            return dt;
        }

        public static DataTable SubmitDeviceBatch(List<MSDeviceDetail> list)
        {
            var dt = new DataTable();
            dt.Columns.Add("@DeviceCode", typeof(string));
            dt.Columns.Add("@PlantCode", typeof(string));
            dt.Columns.Add("@CourseCode", typeof(string));
            dt.Columns.Add("@IsChecked", typeof(bool));

            foreach (var item in list)
            {
                dt.Rows.Add(item.DeviceCode, item.PlantCode, item.CourseCode , item.IsChecked);
            }

            return dt;
        }

        public static DataTable ToDataCheckLeader(List<InspectionDataLeaderBatch> list)
        {
            var dt = new DataTable();
            dt.Columns.Add("@InspectionNo", typeof(string));
            dt.Columns.Add("@CourseCode", typeof(string));
            dt.Columns.Add("@GroupCode", typeof(string));
            dt.Columns.Add("@InspectionDate", typeof(DateTime));
            dt.Columns.Add("@Frequency",typeof(string));

            foreach (var item in list)
            {
                dt.Rows.Add(item.InspectionNo, item.CourseCode, item.GroupCode, item.InspectionDate, item.Frequency);
            }

            return dt;
        }

        public static DataTable ToDataApproval(List<InspectionDataApproveDetail> list)
        {
            var dt = new DataTable();
            dt.Columns.Add("@InspectionNo", typeof(string));
            dt.Columns.Add("@CourseCode", typeof(string));
            dt.Columns.Add("@GroupCode", typeof(string));
            dt.Columns.Add("@InspectionDate", typeof(DateTime));
            dt.Columns.Add("@Frequency", typeof(string));

            foreach (var item in list)
            {
                dt.Rows.Add(item.InspectionNo, item.CourseCode , item.GroupCode, item.InspectionDate, item.Frequency);
            }

            return dt;
        }

        public static DataTable ToUserCourseTable(List<UserSettingCourse> list)
        {
            var dt = new DataTable();
            dt.Columns.Add("UserID", typeof(string));
            dt.Columns.Add("PlantCode", typeof(string));
            dt.Columns.Add("CourseCode", typeof(string));
            dt.Columns.Add("AllowAccess", typeof(bool));

            foreach (var item in list)
            {
                dt.Rows.Add(item.UserID,item.PlantCode,item.CourseCode, item.AllowAccess);
            }

            return dt;
        }

        /*MOBILE*/
        public static DataTable ToHeaderTable(List<HeaderDto> list)
        {
            var dt = new DataTable();

            dt.Columns.Add("InspectionNo");
            dt.Columns.Add("Inspection_Date", typeof(DateTime));
            dt.Columns.Add("HT_Code");
            dt.Columns.Add("Plant_Code");
            dt.Columns.Add("Course_Code");
            dt.Columns.Add("Group_Code");
            dt.Columns.Add("Inspector_Status");
            dt.Columns.Add("Inspector_User");
            dt.Columns.Add("Inspector_Date", typeof(DateTime));
            dt.Columns.Add("Shift_Cls");
            dt.Columns.Add("Notes");
            dt.Columns.Add("Group_Selection");
            dt.Columns.Add("Leader_User");
            dt.Columns.Add("Supervisor_User");
            dt.Columns.Add("Register_User");
            dt.Columns.Add("Register_Date");
            dt.Columns.Add("Last_User");
            dt.Columns.Add("Last_Update");

            foreach (var x in list)
            {
                dt.Rows.Add(
                    x.InspectionNo,
                    x.Inspection_Date,
                    x.HT_Code,
                    x.Plant_Code,
                    x.Course_Code,
                    x.Group_Code,
                    x.Inspector_Status,
                    x.Inspector_User,
                    x.Inspector_Date,
                    x.Shift_Cls,
                    x.Notes,
                    x.Group_Selection,
                    x.Leader_User,
                    x.Supervisor_User,
                    x.Register_User,
                    x.Register_Date,
                    x.Last_User,
                    x.Last_Update
                );
            }

            return dt;
        }
        public static DataTable ToDetailTable(List<DetailDto> list)
        {
            var dt = new DataTable();

            dt.Columns.Add("SequenceNo", typeof(int));
            dt.Columns.Add("InspectionNo");
            dt.Columns.Add("Inspection_Code");
            dt.Columns.Add("Group_Code");
            dt.Columns.Add("Unit");
            dt.Columns.Add("Inspection_Result", typeof(double));
            dt.Columns.Add("Inspection_Result_Option");
            dt.Columns.Add("Status");
            dt.Columns.Add("Register_User");
            dt.Columns.Add("Register_Date");
            dt.Columns.Add("Last_User");
            dt.Columns.Add("Last_Date");

            foreach (var x in list)
            {
                dt.Rows.Add(
                    x.SequenceNo,
                    x.InspectionNo,
                    x.Inspection_Code,
                    x.Group_Code,
                    x.Unit,
                    x.Inspection_Result,
                    x.Inspection_Result_Option,
                    x.Status,
                    x.Register_User,
                    x.Register_Date,
                    x.Last_User,
                    x.Last_Date
                );
            }

            return dt;
        }
    }
}
