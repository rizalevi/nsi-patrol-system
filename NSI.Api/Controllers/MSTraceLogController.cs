using ClosedXML.Excel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NSI.Domain.DTOs.Report;
using NSI.Domain.Interfaces.Report;

namespace NSI.Api.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/mstracelog")]
    public class MSTraceLogController : CommonController
    {
        private readonly IMSTraceLogRepository _repo;

        public MSTraceLogController(IMSTraceLogRepository repo)
        {
            _repo = repo;
        }

        // Endpoint untuk memuat data grid
        [HttpPost("list")]
        public async Task<IActionResult> ListData([FromBody] TraceLogExportParameterDto payload)
        {
            var results = await _repo.GetAllAsync(payload.Module, payload.LogDateFrom, payload.LogDateTo);
            return Success(results);
        }

        // Endpoint untuk memuat combo box Module
        [HttpPost("getcombo")]
        public async Task<IActionResult> GetCombo()
        {
            // Asumsi Anda memiliki method GetComboAsync() di Repositories
            var results = await _repo.GetComboAsync();
            return Success(results);
        }

        // Endpoint khusus untuk memproses dan mendownload Excel
        [HttpPost("export")]
        public async Task<IActionResult> ExportToExcel([FromBody] TraceLogExportParameterDto payload)
        {
            // 1. Ambil data dari service menggunakan parameter yang sama dengan ListData
            var data = await _repo.GetAllAsync(payload.Module, payload.LogDateFrom, payload.LogDateTo);

            using (var workbook = new XLWorkbook())
            {
                var worksheet = workbook.Worksheets.Add("C06");
                int totalColumns = 8; // Sesuai dengan kolom MSTraceLogDto

                // ==========================================
                // HEADER & FORMATTING EXCEL (Baris 1 - 4)
                // ==========================================

                var titleCell = worksheet.Cell(1, 1);
                titleCell.Value = "C06 - Trace Log Master";
                titleCell.Style.Font.Bold = true;
                titleCell.Style.Font.FontSize = 18;
                worksheet.Range(1, 1, 1, totalColumns).Merge();

                var moduleCell = worksheet.Cell(3, 1);
                moduleCell.Value = $"Module  :  {payload.Module}  -  {payload.ModuleName}";
                moduleCell.Style.Font.Bold = true;
                moduleCell.Style.Font.FontSize = 9;
                worksheet.Range(3, 1, 3, totalColumns).Merge();

                var dateCell = worksheet.Cell(4, 1);
                dateCell.Value = $"Log Date  :  {payload.LogDateFrom}  To  {payload.LogDateTo}";
                dateCell.Style.Font.Bold = true;
                dateCell.Style.Font.FontSize = 9;
                worksheet.Range(4, 1, 4, totalColumns).Merge();

                // ==========================================
                // HEADER KOLOM TABEL (Baris 6)
                // ==========================================

                int currentRow = 6;

                worksheet.Cell(currentRow, 1).Value = "Log Date";
                worksheet.Cell(currentRow, 2).Value = "User ID";
                worksheet.Cell(currentRow, 3).Value = "User Name";
                worksheet.Cell(currentRow, 4).Value = "IP Address";
                worksheet.Cell(currentRow, 5).Value = "Module ID";
                worksheet.Cell(currentRow, 6).Value = "Module";
                worksheet.Cell(currentRow, 7).Value = "Action";
                worksheet.Cell(currentRow, 8).Value = "Event Log";

                var headerRange = worksheet.Range(currentRow, 1, currentRow, totalColumns);
                headerRange.Style.Fill.BackgroundColor = XLColor.LightBlue;
                headerRange.Style.Font.Bold = true;
                headerRange.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;

                currentRow++;

                // ==========================================
                // ISI DATA
                // ==========================================

                foreach (var item in data)
                {
                    // Pastikan format tipe data cocok, ClosedXML pintar membaca string/date
                    worksheet.Cell(currentRow, 1).Value = item.LogDate;
                    worksheet.Cell(currentRow, 2).Value = item.UserID;
                    worksheet.Cell(currentRow, 3).Value = item.UserName;
                    worksheet.Cell(currentRow, 4).Value = item.IPAddress;
                    worksheet.Cell(currentRow, 5).Value = item.ModuleID;
                    worksheet.Cell(currentRow, 6).Value = item.Module;
                    worksheet.Cell(currentRow, 7).Value = item.Action;
                    worksheet.Cell(currentRow, 8).Value = item.EventLog;

                    currentRow++;
                }

                // ==========================================
                // STYLING BORDERS & AUTOFIT
                // ==========================================

                if (data.Any())
                {
                    var dataRange = worksheet.Range(6, 1, currentRow - 1, totalColumns);
                    dataRange.Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
                    dataRange.Style.Border.InsideBorder = XLBorderStyleValues.Thin;
                }

                worksheet.Columns(1, totalColumns).AdjustToContents(); // AutoFit kolom

                // ==========================================
                // GENERATE FILE RETURN
                // ==========================================

                using (var stream = new MemoryStream())
                {
                    workbook.SaveAs(stream);
                    var content = stream.ToArray();

                    string timestamp = DateTime.Now.ToString("yyyyMMdd-HHmmss");
                    string fileName = $"C06 - Trace Log Master_{timestamp}.xlsx";
                    string contentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";

                    // Karena ini adalah file download, kita me-return File, bukan Success() API JSON
                    return File(content, contentType, fileName);
                }
            }
        }
    }
}