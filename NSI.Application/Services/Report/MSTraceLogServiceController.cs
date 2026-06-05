using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NSI.Domain.DTOs.Report;
using NSI.Infra.Helpers;
using System.Text.Json;

namespace NSI.Application.Services.Report
{
    [ApiController, Route("service/mstracelog")]
    public class MSTraceLogServiceController(HttpRequestHelper htr) : Controller
    {
        [Authorize, HttpPost, Route("list")]
        public async Task<IActionResult> GetList([FromBody] TraceLogExportParameterDto model)
        {
            var result = await htr.PostAsyncDataTable<List<MSTraceLogDto>>(
                JsonSerializer.Serialize(model),
                "api/mstracelog/list"
            );

            return Json(new { data = result.Data });
        }

        [Authorize, HttpPost, Route("getcombo")]
        public async Task<IActionResult> GetCombo()
        {
            var result = await htr.PostAsyncDataTable<List<MSTraceLogComboDto>>(
                "{}",
                "api/mstracelog/getcombo"
            );

            return Json(new { data = result.Data });
        }

        [Authorize, HttpPost, Route("export")]
        public async Task<IActionResult> ExportToExcel([FromBody] TraceLogExportParameterDto model)
        {
            var fileBytes = await htr.PostAsyncAsByteArray(
                JsonSerializer.Serialize(model),
                "api/mstracelog/export"
            );

            if (fileBytes != null && fileBytes.Length > 0)
            {
                string fileName = $"TraceLogMaster_{DateTime.Now:yyyyMMdd_HHmmss}.xlsx";
                return File(fileBytes, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", fileName);
            }

            return BadRequest("Gagal membuat atau mengunduh file Excel dari API.");
        }
    }
}