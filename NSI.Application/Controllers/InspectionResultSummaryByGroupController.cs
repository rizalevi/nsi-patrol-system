using Microsoft.AspNetCore.Mvc;
using NSI.Domain.DTOs.InspectionResult;
using OfficeOpenXml.Style;
using OfficeOpenXml;
using NSI.Infra.Helpers;
using System.Text.Json;
using Microsoft.AspNetCore.Authorization;
using NSI.Application.Filters;

namespace NSI.Application.Controllers
{

    [Route("InspectionResultSummaryByGroup")]
    public class InspectionResultSummaryByGroupController : BaseController
    {

        //private readonly HttpRequestHelper _htr;

        //public InspectionResultSummaryByGroupController(HttpRequestHelper htr)
        //{
        //    //  _authApi = authApi;
        //    _htr = htr;

        //}
        public InspectionResultSummaryByGroupController(HttpRequestHelper htr) : base(htr) { }


        [Authorize]
        [AuthorizeMenu]
        [HttpGet("index")]
        public async Task<IActionResult> Index(string ID)
        {
            var permission = await GetPrivilegeAsync(ID);
            

            ViewBag.Permission = permission;

            return View();
        }

        [HttpPost("detail")]
        public async Task<IActionResult> Detail(string ID, string ClusterCode, string PlantCode, DateOnly InspectionDateFrom, DateOnly InspectionDateTo)
        {
            var permission = await GetPrivilegeAsync(ID);
            ViewBag.Permission = permission;

            ViewData["ClusterCode"] = ClusterCode;
            ViewData["PlantCode"] = PlantCode;
            ViewData["InspectionDateFrom"] = InspectionDateFrom;
            ViewData["InspectionDateTo"] = InspectionDateTo;
            return View("Index");
        }


        private async Task<List<InspectionResultSummaryByGroupDto>> GetListData(InspectionResultByGroupParamDto model)
        {


            var result = await _htr.PostAsyncDataTable<List<InspectionResultSummaryByGroupDto>>(
                 JsonSerializer.Serialize(model),
               "api/inspectionresultbygroup/get-data"
            );

            return result.Data.ToList();

        }

        [HttpPost("exportexcel")]
        public async Task<IActionResult> ExportExcel([FromBody] InspectionResultByGroupExcelDto model)
        {

            InspectionResultByGroupParamDto payload = new InspectionResultByGroupParamDto();
            payload.ClusterCode = model.ClusterCode;
            payload.PlantCode = model.PlantCode;
            payload.InspectionDateFrom = model.InspectionDateFrom;
            payload.InspectionDateTo = model.InspectionDateTo;

            var listData = await GetListData(payload);

            using (var package = new ExcelPackage())
            {
                var ws = package.Workbook.Worksheets.Add("Inspection Result Summary By Group");

                ws.Cells.Style.Font.Size = 11;

                // ===== HEADER =====
                ws.Cells["A1"].Value = "INSPECTION RESULT ANALYSIS ( SUMMARY BY EQUIPMENT GROUP)";
                ws.Cells["A1"].Style.Font.Bold = true;
                ws.Cells["A1"].Style.Font.Size = 16;
                ws.Cells["A1:F1"].Merge = true;

                ws.Cells["A3"].Value = $"Cluster : {model.ClusterName}";
                ws.Cells["A5"].Value = $"Plant   : {model.PlantName}";
                ws.Cells["A7"].Value = $"Inspection Date : {model.InspectionDateFrom:dd MMM yyyy} ~ {model.InspectionDateTo:dd MMM yyyy}";

                // ===== TABLE HEADER =====
                ws.Cells["A9"].Value = "No";
                ws.Cells["B9"].Value = "Course";
                ws.Cells["C9"].Value = "Equipment(Group)";
                ws.Cells["D9"].Value = "Abnormal Frequency";
                ws.Cells["A9:D9"].Style.Font.Bold = true;


                // ===== DATA =====
                int row = 10;
                int rno = 1;
                foreach (var item in listData)
                {
                    ws.Cells[row, 1].Value = rno;
                    ws.Cells[row, 2].Value = item.CourseName;
                    ws.Cells[row, 3].Value = item.GroupName;
                    ws.Cells[row, 4].Value = item.AbnormalFrequency;

                    row++;
                    rno++;
                }

                // ===== STYLING =====
                var range = ws.Cells[9, 1, row - 1, 5]; //[11, 1, row - 1, 14];
                range.Style.Border.Top.Style = ExcelBorderStyle.Thin;
                range.Style.Border.Left.Style = ExcelBorderStyle.Thin;
                range.Style.Border.Right.Style = ExcelBorderStyle.Thin;
                range.Style.Border.Bottom.Style = ExcelBorderStyle.Thin;


                ws.Cells[9, 1].Style.Font.Bold = true;
                ws.Cells[9, 1].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                ws.Cells[9, 1].Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                // ===== AUTO FIT FIX =====
                ws.Cells[9, 1, row - 1, 4].AutoFitColumns();

                // kasih padding biar ga mepet
                for (int col = 1; col <= 5; col++)
                {
                    ws.Column(col).Width += 2;
                }


                // freeze header
                ws.View.FreezePanes(9, 1);

                // ===== RETURN FILE =====
                var fileBytes = package.GetAsByteArray();

                var fileName = $"INSPECTION_RESULT_SUMMARY_BYGROUP_{DateTime.Now:yyyyMMddHHmm}.xlsx";
                Response.Headers["Content-Disposition"] = $"attachment; filename={fileName}";

                return File(fileBytes, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", fileName);
            }
        }
    }
}
