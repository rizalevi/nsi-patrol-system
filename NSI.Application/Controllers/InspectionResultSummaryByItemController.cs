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

    [Route("InspectionResultSummaryByItem")]
    public class InspectionResultSummaryByItemController : BaseController
    {
        //private readonly HttpRequestHelper _htr;

        //public InspectionResultSummaryByItemController(HttpRequestHelper htr)
        //{
        //    //  _authApi = authApi;
        //    _htr = htr;

        //}
        public InspectionResultSummaryByItemController(HttpRequestHelper htr) : base(htr) { }

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
        public async Task<IActionResult> Detail(string ID, string ClusterCode, string PlantCode,string CourseCode, string GroupCode, DateOnly InspectionDateFrom, DateOnly InspectionDateTo)
        {
            var permission = await GetPrivilegeAsync(ID);
            ViewBag.Permission = permission;


            ViewData["ClusterCode"] = ClusterCode;
            ViewData["PlantCode"] = PlantCode;
            ViewData["CourseCode"] = CourseCode;
            ViewData["GroupCode"] = GroupCode;
            ViewData["InspectionDateFrom"] = InspectionDateFrom;
            ViewData["InspectionDateTo"] = InspectionDateTo;
            return View("Index");
        }

        private async Task<List<InspectionResultSummaryByItemDto>> GetListData(InspectionResultByItemParamDto model)
        {


            var result = await _htr.PostAsyncDataTable<List<InspectionResultSummaryByItemDto>>(
                 JsonSerializer.Serialize(model),
               "api/inspectionresultbyitem/get-data"
            );

            return result.Data.ToList();

        }

        [HttpPost("exportexcel")]
        public async Task<IActionResult> ExportExcel([FromBody] InspectionResultByItemExcelDto model)
        {
            InspectionResultByItemParamDto payload = new InspectionResultByItemParamDto();
            payload.CourseCode = model.CourseCode;
            payload.GroupCode = model.GroupCode;
            payload.InspectionDateFrom = model.InspectionDateFrom;
            payload.InspectionDateTo = model.InspectionDateTo;


            var listData = await GetListData(payload);

            using (var package = new ExcelPackage())
            {
                var ws = package.Workbook.Worksheets.Add("Inspection Result Summary By Item");

                ws.Cells.Style.Font.Size = 11;

                // ===== HEADER =====
                ws.Cells["A1"].Value = "INSPECTION RESULT ANALYSIS ( SUMMARY BY ITEM)";
                ws.Cells["A1"].Style.Font.Bold = true;
                ws.Cells["A1"].Style.Font.Size = 16;
                ws.Cells["A1:F1"].Merge = true;

                ws.Cells["A3"].Value = $"Course : {model.CourseName}";
                ws.Cells["A5"].Value = $"Group   : {model.GroupName}";
                ws.Cells["A7"].Value = $"Inspection Date : {model.InspectionDateFrom:dd MMM yyyy} ~ {model.InspectionDateTo:dd MMM yyyy}";

                // ===== TABLE HEADER =====
                ws.Cells["A9"].Value = "No";
                ws.Cells["B9"].Value = "Inspection Point";
                ws.Cells["C9"].Value = "Abnormal Frequency";
                ws.Cells["A9:C9"].Style.Font.Bold = true;


                // ===== DATA =====
                int row = 10;
                int rno = 1;
                foreach (var item in listData)
                {
                    ws.Cells[row, 1].Value = rno;
                    ws.Cells[row, 2].Value = item.InspectionPoint;
                    ws.Cells[row, 3].Value = item.AbnormalFrequency;

                    row++;
                    rno++;
                }

                // ===== STYLING =====
                var range = ws.Cells[9, 1, row - 1, 3]; //[11, 1, row - 1, 14];
                range.Style.Border.Top.Style = ExcelBorderStyle.Thin;
                range.Style.Border.Left.Style = ExcelBorderStyle.Thin;
                range.Style.Border.Right.Style = ExcelBorderStyle.Thin;
                range.Style.Border.Bottom.Style = ExcelBorderStyle.Thin;


                ws.Cells[9, 1].Style.Font.Bold = true;
                ws.Cells[9, 1].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                ws.Cells[9, 1].Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                // ===== AUTO FIT FIX =====
                ws.Cells[9, 1, row - 1, 3].AutoFitColumns();

                // kasih padding biar ga mepet
                for (int col = 1; col <= 4; col++)
                {
                    ws.Column(col).Width += 2;
                }


                // freeze header
                ws.View.FreezePanes(9, 1);

                // ===== RETURN FILE =====
                var fileBytes = package.GetAsByteArray();

                var fileName = $"INSPECTION_RESULT_SUMMARY_BYITEM_{DateTime.Now:yyyyMMddHHmm}.xlsx";
                Response.Headers["Content-Disposition"] = $"attachment; filename={fileName}";

                return File(fileBytes, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", fileName);
            }
        }
    }
}
