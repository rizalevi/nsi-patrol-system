using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NSI.Domain.DTOs;
using NSI.Domain.DTOs.Inspection;
using NSI.Domain.DTOs.InspectionResult;
using NSI.Infra.Helpers;
using OfficeOpenXml;
using OfficeOpenXml.Style;
using System.Text.Json;

namespace NSI.Application.Controllers
{

    public class InspectionResultSumByItemController : Controller
    {
        private readonly HttpRequestHelper _htr;

        public InspectionResultSumByItemController(HttpRequestHelper htr)
        {
            //  _authApi = authApi;
            _htr = htr;

        }

        [HttpGet("index")]
        public IActionResult Index()
        {

            return View();
        }

        
        private async Task<List<InspectionResultSummaryByItemDto>> GetListData(InspectionResultParamDto model)
        {


            var result = await _htr.PostAsyncDataTable<List<InspectionResultSummaryByItemDto>>(
                 JsonSerializer.Serialize(model),
                "api/inspectionresultbyitem/getdata"
            );

            return result.Data.ToList();


        }

        //[HttpPost]
        //public async Task<IActionResult> ExportExcel([FromBody] InspectionResultParamDto model)
        //{
        //    var listData = await GetListData(model);

        //    using (var package = new ExcelPackage())
        //    {
        //        var ws = package.Workbook.Worksheets.Add("Inspection Result Summary By Item");

        //        ws.Cells.Style.Font.Size = 11;


        //        // ===== HEADER =====
        //        ws.Cells["A1"].Value = "INSPECTION RESULT SUMMARY BY ITEM";
        //        ws.Cells["A1"].Style.Font.Bold = true;
        //        ws.Cells["A1"].Style.Font.Size = 16;
        //        ws.Cells["A1:F1"].Merge = true;

        //        ws.Cells["A3"].Value = $"Cluster : {model.ClusterCode}";
        //        ws.Cells["D3"].Value = $"Inspection Point : {model.InspectionPointCode}";
        //        ws.Cells["A5"].Value = $"Plant : {model.PlantCode}";
        //        ws.Cells["D5"].Value = $"Frequency : {model.Frequency}";
        //        ws.Cells["A7"].Value = $"Course : {model.CourseCode}";
        //        ws.Cells["D7"].Value = $"Inspection Date : {model.InspectionDateFrom:dd MMM yyyy} ~ {model.InspectionDateTo:dd MMM yyyy}";
        //        ws.Cells["A9"].Value = $"Group : {model.GroupCode}";

        //        // Merge
        //        ws.Cells["A3:C3"].Merge = true;
        //        ws.Cells["D3:F3"].Merge = true;
        //        ws.Cells["A5:C5"].Merge = true;
        //        ws.Cells["D5:F5"].Merge = true;
        //        ws.Cells["A7:C7"].Merge = true;
        //        ws.Cells["D7:G7"].Merge = true;
        //        ws.Cells["A9:C9"].Merge = true;

        //        // ===== TABLE HEADER =====
        //        ws.Cells["A11"].Value = "Inspection Date";
        //        ws.Cells["B11"].Value = "Frequency";
        //        ws.Cells["C11"].Value = "Plant";
        //        ws.Cells["D11"].Value = "Course";
        //        ws.Cells["E11"].Value = "Equipment Group";
        //        ws.Cells["F11"].Value = "Inspection Point";
        //        ws.Cells["G11"].Value = "Unit";
        //        ws.Cells["H11"].Value = "Range";
        //        ws.Cells["J11"].Value = "Inspection Result";
        //        ws.Cells["K11"].Value = "Status";
        //        ws.Cells["L11"].Value = "Group Selection";
        //        ws.Cells["M11"].Value = "Inspector User";
        //        ws.Cells["N11"].Value = "Notes";

        //        // Sub header
        //        ws.Cells["H12"].Value = "Min";
        //        ws.Cells["I12"].Value = "Max";

        //        // Merge header
        //        ws.Cells["A11:A12"].Merge = true;
        //        ws.Cells["B11:B12"].Merge = true;
        //        ws.Cells["C11:C12"].Merge = true;
        //        ws.Cells["D11:D12"].Merge = true;
        //        ws.Cells["E11:E12"].Merge = true;
        //        ws.Cells["F11:F12"].Merge = true;
        //        ws.Cells["G11:G12"].Merge = true;
        //        ws.Cells["H11:I11"].Merge = true;
        //        ws.Cells["J11:J12"].Merge = true;
        //        ws.Cells["K11:K12"].Merge = true;
        //        ws.Cells["L11:L12"].Merge = true;
        //        ws.Cells["M11:M12"].Merge = true;
        //        ws.Cells["N11:N12"].Merge = true;

        //        // ===== DATA =====
        //        int row = 13;
        //        foreach (var item in listData)
        //        {
        //            ws.Cells[row, 1].Value = item.InspectionDate;
        //            ws.Cells[row, 1].Style.Numberformat.Format = "dd MMM yyyy";
        //            ws.Cells[row, 2].Value = item.Frequency;
        //            ws.Cells[row, 3].Value = item.PlantCode;
        //            ws.Cells[row, 4].Value = item.CourseCode;
        //            ws.Cells[row, 5].Value = item.Group;
        //            ws.Cells[row, 6].Value = item.InspectionPoint;
        //            ws.Cells[row, 7].Value = item.Unit;
        //            ws.Cells[row, 8].Value = item.Minimum;
        //            ws.Cells[row, 9].Value = item.Maximum;
        //            ws.Cells[row, 10].Value = item.InspectionResult;
        //            ws.Cells[row, 11].Value = item.Status;
        //            ws.Cells[row, 12].Value = item.GroupSelection;
        //            ws.Cells[row, 13].Value = item.InspectorUser;
        //            ws.Cells[row, 14].Value = item.Notes;
        //            // 🔥 HIGHLIGHT ABNORMAL
        //            if (!string.IsNullOrEmpty(item.Status) && item.Status.ToLower() == "abnormal")
        //            {
        //                using (var rangeRow = ws.Cells[row, 1, row, 14])
        //                {
        //                    rangeRow.Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
        //                    rangeRow.Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.Yellow);
        //                }
        //            }
        //            row++;
        //        }

        //        // ===== STYLING =====
        //        var range = ws.Cells[11, 1, row - 1, 14];
        //        range.Style.Border.Top.Style = ExcelBorderStyle.Thin;
        //        range.Style.Border.Left.Style = ExcelBorderStyle.Thin;
        //        range.Style.Border.Right.Style = ExcelBorderStyle.Thin;
        //        range.Style.Border.Bottom.Style = ExcelBorderStyle.Thin;

        //        ws.Cells[11, 1, 12, 14].Style.Font.Bold = true;
        //        ws.Cells[11, 1, 12, 14].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
        //        ws.Cells[11, 1, 12, 14].Style.VerticalAlignment = ExcelVerticalAlignment.Center;

        //        // ===== AUTO FIT FIX =====
        //        ws.Cells[11, 1, row - 1, 14].AutoFitColumns();

        //        // kasih padding biar ga mepet
        //        for (int col = 1; col <= 14; col++)
        //        {
        //            ws.Column(col).Width += 2;
        //        }

        //        // khusus notes biar enak dibaca
        //        ws.Column(14).Style.WrapText = true;
        //        ws.Column(14).Width = 30;

        //        // freeze header
        //        ws.View.FreezePanes(13, 1);

        //        // ===== RETURN FILE =====
        //        var fileBytes = package.GetAsByteArray();

        //        var fileName = $"INSPECTION_DATA_{DateTime.Now:yyyyMMddHHmm}.xlsx";
        //        Response.Headers["Content-Disposition"] =  $"attachment; filename={fileName}";

        //        return File(fileBytes,"application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", fileName);
        //    }
        //}

    }
}