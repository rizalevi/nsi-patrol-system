using Dapper;
using NSI.Domain.DTOs.Inspection;
using NSI.Domain.Interfaces.Inspection;
using NSI.Infra.Datas;
using System.Data;

namespace NSI.Infra.Repositories.Inspection
{
    public class InspectionDataInquiryRepository : IInspectionDataInquiryRepository
    {
        private readonly DbExecutor _dbExecutor;
       
        public InspectionDataInquiryRepository(DbExecutor dbExecutor)
        {
            _dbExecutor = dbExecutor;
        }

        public async Task<List<InspectionDataInquiryListDto>> GetList(InspectionDataInquiryParamDto model)
        {

            string sp = "sp_WEB_InspectionData_Inquiry_List";
            var result = await _dbExecutor.QueryListAsync<InspectionDataInquiryListDto>(sp, model);
            return result.ToList();

            //string sp = "sp_WEB_InspectionData_Inquiry_List";
            //    var result = await _dbExecutor.QueryListAsync<InspectionDataInquiryListDto>(sp, new { ClusterCode = model.ClusterCode, PlantCode = model.PlantCode ,
            //                                                                                          CourseCode = model.CourseCode , GroupCode = model.GroupCode , InspectionPointCode = model.InspectionPointCode,
            //                                                                                          Frequency = model.Frequency , InspectionDateFrom = model.InspectionFromDate , InspectionDateTo = model.InspectionToDate 
            //                                                                                        }  );
            //    return result.ToList();

            
        }

        public async Task<List<InspectionDataDailyReportDto>> GetReportDaily(InspectionDataInquiryParamDto model) //string courseCode, DateTime inspectionDate)
        {

          
                string sp = "sp_Web_Report_InspectionDataInquiry_Daily";
                var result = await _dbExecutor.QueryListAsync<InspectionDataDailyReportDto>(
                    sp,
                    new
                    {
                        CourseCode = model.CourseCode,
                        InspectionDate = model.InspectionDateFrom,
                        Frequency = model.Frequency
                    },
                    CommandType.StoredProcedure // 🔥 WAJIB
                );
                return result.ToList();
            
               
        }

        public async Task<List<InspectionDataAllReportDto>> GetReportAllShift(InspectionDataInquiryParamDto model) //string courseCode, DateTime inspectionDate)
        {

           
                string sp = "sp_Web_Report_InspectionDataInquiry";

                var result = await _dbExecutor.QueryListAsync<InspectionDataAllReportDto>(
                    sp,
                    new
                    {
                        CourseCode = model.CourseCode,
                        InspectionDate = model.InspectionDateFrom
                    },
                    CommandType.StoredProcedure 
                );

                return result.ToList();
            

        }
    }
}
