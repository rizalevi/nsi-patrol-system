using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NSI.Domain.DTOs.Report;
using NSI.Infra.Helpers;
using System.Text.Json;

namespace NSI.Application.Services.Report
{
    [ApiController, Route("service/topplanabnormality")]
    public class TopPlanAbnormalityServicesController(HttpRequestHelper htr) : Controller
    {
        [Authorize, HttpPost, Route("getdataplant")]
        public async Task<IActionResult> GetList([FromBody] TopPlanAbnormalityPlantParamDto model)
        {
            var result = await htr.PostAsyncDataTable<List<TopPlanAbnormalityPlantDto>>(JsonSerializer.Serialize(model), "api/topplanabnormality/getdataplant");
            return Json(new { data = result.Data });
        }

        [Authorize, HttpPost, Route("getchart-byplant")]
        public async Task<IActionResult> GetChartByPlant([FromBody] TopPlanAbnormalityPlantParamDto model)
        {
            var result = await htr.PostAsyncDataTable<List<TopPlanAbnormalityPlantDto>>(JsonSerializer.Serialize(model), "api/topplanabnormality/getchart-byplant");
            return Json(new { data = result.Data });
        }

        [Authorize, HttpPost, Route("getdatacourse")]
        public async Task<IActionResult> GetList([FromBody] TopPlanAbnormalityCourseParamDto model)
        {
            var result = await htr.PostAsyncDataTable<List<TopPlanAbnormalityCourseDto>>(JsonSerializer.Serialize(model), "api/topplanabnormality/getdatacourse");
            return Json(new { data = result.Data });
        }

        [Authorize, HttpPost, Route("getchart-bycourse")]
        public async Task<IActionResult> GetChartByCourse([FromBody] TopPlanAbnormalityCourseParamDto model)
        {
            var result = await htr.PostAsyncDataTable<List<TopPlanAbnormalityCourseDto>>(JsonSerializer.Serialize(model), "api/topplanabnormality/getchart-bycourse");
            return Json(new { data = result.Data });
        }

        [Authorize, HttpPost, Route("getdatainspection")]
        public async Task<IActionResult> GetList([FromBody] TopPlanAbnormalityInspectionParamDto model)
        {
            var result = await htr.PostAsyncDataTable<List<TopPlanAbnormalityInspectionDto>>(JsonSerializer.Serialize(model), "api/topplanabnormality/getdatainspection");
            return Json(new { data = result.Data });
        }

        [Authorize, HttpPost, Route("exportplant")]
        public async Task<IActionResult> ExportTopPlanAbnormalityPlantToExcel([FromBody] TopPlanAbnormalityPlantParamDto model)
        {
            // Meneruskan request ke API utama
            var fileBytes = await htr.PostAsyncAsByteArray(
                JsonSerializer.Serialize(model),
                "api/topplanabnormality/exportplant" // Pastikan endpoint ini sesuai dengan API yang kita buat sebelumnya
            );

            // Cek apakah balasan dari API valid dan memiliki isi byte
            if (fileBytes != null && fileBytes.Length > 0)
            {
                // [Perbaikan]: Ubah nama file dari TraceLogMaster menjadi TopPlanAbnormality_Plant
                string fileName = $"TopPlanAbnormality_Plant_{DateTime.Now:yyyyMMdd_HHmmss}.xlsx";

                return File(fileBytes, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", fileName);
            }

            return BadRequest("Gagal membuat atau mengunduh file Excel dari API.");
        }

        [Authorize, HttpPost, Route("exportcourse")]
        public async Task<IActionResult> ExportTopPlanAbnormalityCourseToExcel([FromBody] TopPlanAbnormalityCourseParamDto model)
        {
            // Meneruskan request ke API utama
            var fileBytes = await htr.PostAsyncAsByteArray(
                JsonSerializer.Serialize(model),
                "api/topplanabnormality/exportcourse" // Pastikan endpoint ini sesuai dengan API yang kita buat sebelumnya
            );

            // Cek apakah balasan dari API valid dan memiliki isi byte
            if (fileBytes != null && fileBytes.Length > 0)
            {
                // [Perbaikan]: Ubah nama file dari TraceLogMaster menjadi TopPlanAbnormality_Plant
                string fileName = $"TopPlanAbnormality_Course_{DateTime.Now:yyyyMMdd_HHmmss}.xlsx";

                return File(fileBytes, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", fileName);
            }

            return BadRequest("Gagal membuat atau mengunduh file Excel dari API.");
        }

        [Authorize, HttpPost, Route("exportinspection")]
        public async Task<IActionResult> ExportTopPlanAbnormalityInspectionToExcel([FromBody] TopPlanAbnormalityInspectionParamDto model)
        {
            // Meneruskan request ke API utama
            var fileBytes = await htr.PostAsyncAsByteArray(
                JsonSerializer.Serialize(model),
                "api/topplanabnormality/exportinspection" // Pastikan endpoint ini sesuai dengan API yang kita buat sebelumnya
            );

            // Cek apakah balasan dari API valid dan memiliki isi byte
            if (fileBytes != null && fileBytes.Length > 0)
            {
                // [Perbaikan]: Ubah nama file dari TraceLogMaster menjadi TopPlanAbnormality_Plant
                string fileName = $"TopPlanAbnormality_Inspection_{DateTime.Now:yyyyMMdd_HHmmss}.xlsx";

                return File(fileBytes, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", fileName);
            }

            return BadRequest("Gagal membuat atau mengunduh file Excel dari API.");

        }
    }
}