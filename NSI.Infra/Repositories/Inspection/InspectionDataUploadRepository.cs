using Dapper;
using NSI.Domain.DTOs.Inspection;
using NSI.Domain.Interfaces.Inspection;
using NSI.Domain.Models.Inspection;
using NSI.Domain.Models.Tasklist;
using NSI.Infra.Datas;
using NSI.Infra.Helpers;
using System.Data;

namespace NSI.Infra.Repositories.Inspection
{
    public class InspectionDataUploadRepository : IInspectionDataUploadRepository
    {
        private readonly DbExecutor _dbExecutor;
       
        public InspectionDataUploadRepository(DbExecutor dbExecutor)
        {
            _dbExecutor = dbExecutor;
        }

        public async Task<List<InspectionDataUploadListDto>> GetList(InspectionDataUploadParamDto model, string userLogin)
        {
            string sp = "sp_Web_InspectionDataUpload_GetList";

            var param = new DynamicParameters();
            param.Add("@PlantCode", model.PlantCode);
            param.Add("@CourseCode", model.CourseCode);
            param.Add("@GroupCode", model.GroupCode);
            param.Add("@Shift", model.Shift);
            param.Add("@InspectionDate", model.InspectionDate.HasValue
                ? model.InspectionDate.Value.ToString("yyyy-MM-dd")  
                : null);
            param.Add("@UserID", userLogin);

            var result = await _dbExecutor.QueryListAsync<InspectionDataUploadListDto>(sp, param);
            return result.ToList();
        }

        public async Task<List<InspectionDataUploadRemarksDto>> GetRemarksByLeader(InspectionDataUploadParamDto model)
        {
            string sp = "sp_Web_InspectionDataUpload_GetRemarks";

            var param = new DynamicParameters();
            param.Add("@PlantCode", model.PlantCode);
            param.Add("@CourseCode", model.CourseCode);
            param.Add("@GroupCode", model.GroupCode);
            param.Add("@Shift", model.Shift);
            param.Add("@InspectionNo", model.InspectionNo);
            param.Add("@InspectionDate", model.InspectionDate.HasValue
                ? model.InspectionDate.Value.ToString("yyyy-MM-dd")  
                : null);
            param.Add("@Type", model.Type);

            var result = await _dbExecutor.QueryListAsync<InspectionDataUploadRemarksDto>(sp, param);
            return result.ToList();
        }

        public async Task<ResponseApprove> ApproveInspector(InspectionDataUploadParamDto model, string userLogin)
        {
            var param = new DynamicParameters();
            param.Add("@PlantCode", model.PlantCode);
            param.Add("@CourseCode", model.CourseCode);
            param.Add("@GroupCode", model.GroupCode);
            param.Add("@Shift", model.Shift);
            param.Add("@InspectionNo", model.InspectionNo);
            param.Add("@InspectionDate", model.InspectionDate.HasValue
                ? model.InspectionDate.Value.ToString("yyyy-MM-dd")  
                : null);
            param.Add("@UserID", userLogin);

            var result = await _dbExecutor.ExecuteAsyncApproval(
                "sp_Web_InspectionDataUpload_ApproveInspector",
                param,
                commandType: CommandType.StoredProcedure,
                commandTimeout: 120
            );


            return result ?? new ResponseApprove();
        }

        public async Task<List<InspectionDataUploadRemarksDto>> GetLastStatus(InspectionDataUploadParamDto model)
        {
            string sp = "sp_Web_InspectionDataUpload_GetLastStatus";

            var param = new DynamicParameters();
            param.Add("@PlantCode", model.PlantCode);
            param.Add("@CourseCode", model.CourseCode);
            param.Add("@GroupCode", model.GroupCode);
            param.Add("@Shift", model.Shift);
            param.Add("@InspectionNo", model.InspectionNo);
            param.Add("@InspectionDate", model.InspectionDate.HasValue
                ? model.InspectionDate.Value.ToString("yyyy-MM-dd")  // kirim sebagai string, bukan DateTime
                : null);
       
            var result = await _dbExecutor.QueryListAsync<InspectionDataUploadRemarksDto>(sp, param);
            return result.ToList();
        }

        public async Task<InspectionDataUploadRemarksDto> CheckStatusLeader(InspectionDataUploadParamDto model)
        {
            string sp = "sp_Web_InspectionDataUpload_CheckStatusLeader";

            var param = new DynamicParameters(); 
            param.Add("@CourseCode", model.CourseCode);
            param.Add("@GroupCode", model.GroupCode);
            param.Add("@ShiftCode", model.Shift);
            param.Add("@InspectionNo", model.InspectionNo);
            param.Add("@InspectionDate", model.InspectionDate.HasValue
                ? model.InspectionDate.Value.ToString("yyyy-MM-dd")  // kirim sebagai string, bukan DateTime
                : null);

            var result = await _dbExecutor.QueryFirstOrDefaultAsync<InspectionDataUploadRemarksDto>(sp, param);
            return result;
        }
         
        public async Task<ResponseApprove> UpdateDetail(InspectionDataUploadUpdateDto model, string ipAddress)
        {
            string safeResult = string.IsNullOrWhiteSpace(model.InspectionResult) ? "0" : model.InspectionResult;

            var param = new DynamicParameters();
            param.Add("@InspectionCode", model.InspectionCode);
            param.Add("@GroupCode", model.GroupCode);
            param.Add("@InspectionNo", model.InspectionNo);
            param.Add("@InspectionResult", safeResult);
            param.Add("@InspectionResultPoint", model.InspectionResultOption);
             
            var getParam = new DynamicParameters();
            getParam.Add("@Inspection_Code", model.InspectionCode);
            getParam.Add("@Group_Code", model.GroupCode);
            getParam.Add("@Inspection_No", model.InspectionNo);
            getParam.Add("@Inspection_Result", safeResult);
             
            var oldData = await _dbExecutor.QueryFirstOrDefaultAsync<dynamic>(
                "sp_Web_InspectionDataUpload_GetImage",
                getParam,
                commandType: CommandType.StoredProcedure
            );
             
            string finalFilePath = oldData?.FileImagePath as string;
            string finalImageName = null;
             
            if (oldData?.FileImageName != null)
            { 
                if (oldData.FileImageName is byte[] bytes)
                {
                    finalImageName = Convert.ToBase64String(bytes);
                }
                else
                { 
                    finalImageName = oldData.FileImageName.ToString();
                }
            }
             
            if (!string.IsNullOrEmpty(model.FileImagePath))
            {
                finalFilePath = model.FileImagePath;
                finalImageName = model.FileNameStr??"";
            }
             
            param.Add("@FileImageName", finalImageName, DbType.String);
            param.Add("@FileImagePath", finalFilePath, DbType.String);
            param.Add("@Last_User", model.LastUser);
            param.Add("@Type", model.Type);
            param.Add("@InspectionResultOptionType", model.InspectionResultOptionType ?? "");
            param.Add("@IPAddress", ipAddress);

            var result = await _dbExecutor.ExecuteAsyncApproval(
                "sp_Web_InspectionDataUpload_SubmitDetail",
                param,
                commandType: CommandType.StoredProcedure,
                commandTimeout: 120
            );

            return result ?? new ResponseApprove();
        }
         
        public async Task<InspectionImageDto> GetImage(InspectionDataUploadUpdateDto model)
        {
            string safeResult = string.IsNullOrWhiteSpace(model.InspectionResult) ? "0" : model.InspectionResult;

            var param = new DynamicParameters();
            param.Add("@Inspection_Code", model.InspectionCode);
            param.Add("@Group_Code", model.GroupCode);
            param.Add("@Inspection_No", model.InspectionNo);
            param.Add("@Inspection_Result", safeResult);
             
            var result = await _dbExecutor.QueryFirstOrDefaultAsync <InspectionImageDto>(
                "sp_Web_InspectionDataUpload_GetImage",
                param,
                commandType: CommandType.StoredProcedure
            );

            if (result == null)
            {
                return null;
            }

            return result;
        }

    }
}
