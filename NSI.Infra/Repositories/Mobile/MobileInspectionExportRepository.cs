using Dapper; 
using NSI.Domain.DTOs.Mobile;
using NSI.Domain.Interfaces.Mobile;
using NSI.Infra.Datas;
using NSI.Infra.Helpers;
using System.Data; 

namespace NSI.Infra.Repositories.Mobile
{
    public class MobileInspectionExportRepository : IMobileInspectionExportRepository
    {
        private readonly DbExecutor _dbExecutor;
        public MobileInspectionExportRepository(DbExecutor dbExecutor)
        {
            _dbExecutor = dbExecutor;
        }
        public async Task<bool> SyncExport(MobileInspectionExportDto data)
        { 

            var headerTable = DataTableHelper.ToHeaderTable(data.Headers);
            var detailTable = DataTableHelper.ToDetailTable(data.Details);
            var exportuser =  data.Export_User ;

            var param = new DynamicParameters();
            param.Add("@Header", headerTable.AsTableValuedParameter("dbo.InspectionHeaderType"));
            param.Add("@Detail", detailTable.AsTableValuedParameter("dbo.InspectionDetailType"));
            param.Add("@Export_User", exportuser);

            await _dbExecutor.ExecuteAsync(
                "sp_Mobile_InspectionExport",
                param,
                commandType: CommandType.StoredProcedure
            );
            return true;    
        }

        //private DataTable ToHeaderTable(List<HeaderDto> list)
        //{
        //    var dt = new DataTable();

        //    dt.Columns.Add("InspectionNo");
        //    dt.Columns.Add("Inspection_Date", typeof(DateTime));
        //    dt.Columns.Add("HT_Code");
        //    dt.Columns.Add("Plant_Code");
        //    dt.Columns.Add("Course_Code");
        //    dt.Columns.Add("Group_Code");
        //    dt.Columns.Add("Inspector_Status");
        //    dt.Columns.Add("Inspector_User");
        //    dt.Columns.Add("Inspector_Date", typeof(DateTime));
        //    dt.Columns.Add("Shift_Cls");
        //    dt.Columns.Add("Notes");
        //    dt.Columns.Add("Group_Selection");
        //    dt.Columns.Add("Leader_User");
        //    dt.Columns.Add("Supervisor_User");
        //    dt.Columns.Add("Register_User");
        //    dt.Columns.Add("Register_Date");
        //    dt.Columns.Add("Last_User");
        //    dt.Columns.Add("Last_Update"); 

        //    foreach (var x in list)
        //    {
        //        dt.Rows.Add(
        //            x.InspectionNo,
        //            x.Inspection_Date,
        //            x.HT_Code,
        //            x.Plant_Code,
        //            x.Course_Code,
        //            x.Group_Code,
        //            x.Inspector_Status,
        //            x.Inspector_User,
        //            x.Inspector_Date,
        //            x.Shift_Cls,
        //            x.Notes,
        //            x.Group_Selection,
        //            x.Leader_User,
        //            x.Supervisor_User,
        //            x.Register_User,
        //            x.Register_Date,
        //            x.Last_User,
        //            x.Last_Update 
        //        );
        //    }

        //    return dt;
        //}


        //private DataTable ToDetailTable(List<DetailDto> list)
        //{
        //    var dt = new DataTable();

        //    dt.Columns.Add("SequenceNo", typeof(int));
        //    dt.Columns.Add("InspectionNo");
        //    dt.Columns.Add("Inspection_Code");
        //    dt.Columns.Add("Group_Code");
        //    dt.Columns.Add("Unit");
        //    dt.Columns.Add("Inspection_Result", typeof(double));
        //    dt.Columns.Add("Inspection_Result_Option");
        //    dt.Columns.Add("Status");
        //    dt.Columns.Add("Register_User");
        //    dt.Columns.Add("Register_Date");
        //    dt.Columns.Add("Last_User");
        //    dt.Columns.Add("Last_Date");

        //    foreach (var x in list)
        //    {
        //        dt.Rows.Add(
        //            x.SequenceNo,
        //            x.InspectionNo,
        //            x.Inspection_Code,
        //            x.Group_Code,
        //            x.Unit,
        //            x.Inspection_Result,
        //            x.Inspection_Result_Option,
        //            x.Status,
        //            x.Register_User,
        //            x.Register_Date,
        //            x.Last_User,
        //            x.Last_Date
        //        );
        //    }

        //    return dt;
        //}
    }
}
