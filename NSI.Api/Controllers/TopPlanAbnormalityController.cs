using ClosedXML.Excel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NSI.Domain.DTOs.Report;
using NSI.Domain.Interfaces.Report;

namespace NSI.Api.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/topplanabnormality")]
    public class TopPlanAbnormalityController : CommonController
    {
        private readonly ITopPlanAbnormalityRepository _repo;

        public TopPlanAbnormalityController(ITopPlanAbnormalityRepository repo)
        {
            _repo = repo;
        }

        /*SCREEN*/

        [HttpPost("getdataplant")]
        public async Task<IActionResult> GetDataPlant([FromBody] TopPlanAbnormalityPlantParamDto model) => Success(await _repo.GetDataPlantAsync(model));


        [HttpPost("getchart-byplant")]
        public async Task<IActionResult> GetChartByPlant([FromBody] TopPlanAbnormalityPlantParamDto model) => Success(await _repo.GetChartByPlant(model));


        [HttpPost("getdatacourse")]
        public async Task<IActionResult> GetDataCourse([FromBody] TopPlanAbnormalityCourseParamDto model) => Success(await _repo.GetDataCourseAsync(model));


        [HttpPost("getchart-bycourse")]
        public async Task<IActionResult> GetChartByCourse([FromBody] TopPlanAbnormalityCourseParamDto model) => Success(await _repo.GetChartByCourse(model));


        [HttpPost("getdatainspection")]
        public async Task<IActionResult> GetDataInspection([FromBody] TopPlanAbnormalityInspectionParamDto model) => Success(await _repo.GetDataInspectionAsync(model));


        /*EXCEL FUNC*/

        [HttpPost("exportplant")]
        public async Task<IActionResult> ExportPlant([FromBody] TopPlanAbnormalityPlantParamDto model)
        {
            var data = await _repo.GetDataPlantAsync(model);

            using (var workbook = new XLWorkbook())
            {
                var worksheet = workbook.Worksheets.Add("Top Plant Abnormality");
                int totalColumns = 5;

                // --- HEADER ---
                var titleCell = worksheet.Cell(1, 1);
                titleCell.Value = "Top Plan Abnormality Report";
                titleCell.Style.Font.Bold = true;
                titleCell.Style.Font.FontSize = 16;
                worksheet.Range(1, 1, 1, totalColumns).Merge();

                var filterCell = worksheet.Cell(3, 1);
                string plantFilter = string.IsNullOrEmpty(model.PlantCode) ? "ALL" : model.PlantCode;
                filterCell.Value = $"Plant Filter :  {plantFilter}";
                filterCell.Style.Font.Bold = true;
                filterCell.Style.Font.FontSize = 10;
                worksheet.Range(3, 1, 3, totalColumns).Merge();

                var dateCell = worksheet.Cell(4, 1);
                dateCell.Value = $"Date Range   :  {model.InspectionDateFrom:dd MMM yyyy}  To  {model.InspectionDateTo:dd MMM yyyy}";
                dateCell.Style.Font.Bold = true;
                dateCell.Style.Font.FontSize = 10;
                worksheet.Range(4, 1, 4, totalColumns).Merge();

                // --- TABEL HEADER ---
                int currentRow = 6;
                worksheet.Cell(currentRow, 1).Value = "Plant Code";
                worksheet.Cell(currentRow, 2).Value = "Plant Name";
                worksheet.Cell(currentRow, 3).Value = "Course Code";
                worksheet.Cell(currentRow, 4).Value = "Course Name";
                worksheet.Cell(currentRow, 5).Value = "Abnormal Frequency";

                var headerRange = worksheet.Range(currentRow, 1, currentRow, totalColumns);
                headerRange.Style.Fill.BackgroundColor = XLColor.LightBlue;
                headerRange.Style.Font.Bold = true;
                headerRange.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;

                currentRow++;

                // --- ISI DATA ---
                if (data != null && data.Any())
                {
                    foreach (var item in data)
                    {
                        worksheet.Cell(currentRow, 1).Value = item.PlantCode;
                        worksheet.Cell(currentRow, 2).Value = item.PlantName;
                        worksheet.Cell(currentRow, 3).Value = item.CourseCode;
                        worksheet.Cell(currentRow, 4).Value= item.CourseName;
                        worksheet.Cell(currentRow, 5).Value = item.AbnormalFrequency;
                        //if (int.TryParse(item.AbnormalFrequency, out int frequency))
                        //    worksheet.Cell(currentRow, 3).Value = frequency;
                        //else
                        //    worksheet.Cell(currentRow, 3).Value = item.AbnormalFrequency;

                        currentRow++;
                    }

                    var dataRange = worksheet.Range(6, 1, currentRow - 1, totalColumns);
                    dataRange.Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
                    dataRange.Style.Border.InsideBorder = XLBorderStyleValues.Thin;
                }

                worksheet.Columns(1, totalColumns).AdjustToContents();

                // --- GENERATE FILE ---
                using (var stream = new MemoryStream())
                {
                    workbook.SaveAs(stream);
                    string fileName = $"Top_Plan_Abnormality_Plant_{DateTime.Now:yyyyMMdd_HHmmss}.xlsx";
                    return File(stream.ToArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", fileName);
                }
            }
        }

        [HttpPost("exportcourse")]
        public async Task<IActionResult> ExportCourse([FromBody] TopPlanAbnormalityCourseParamDto model)
        {
            var data = await _repo.GetDataCourseAsync(model);

            using (var workbook = new XLWorkbook())
            {
                var worksheet = workbook.Worksheets.Add("Top Course Abnormality");
                int totalColumns = 5;

                // --- HEADER ---
                var titleCell = worksheet.Cell(1, 1);
                titleCell.Value = "Top Course Abnormality Report";
                titleCell.Style.Font.Bold = true;
                titleCell.Style.Font.FontSize = 16;
                worksheet.Range(1, 1, 1, totalColumns).Merge();

                var filterCell = worksheet.Cell(3, 1);
                string courseFilter = string.IsNullOrEmpty(model.CourseCode) ? "ALL" : model.CourseCode;
                filterCell.Value = $"Course Filter :  {courseFilter}";
                filterCell.Style.Font.Bold = true;
                filterCell.Style.Font.FontSize = 10;
                worksheet.Range(3, 1, 3, totalColumns).Merge();

                var dateCell = worksheet.Cell(4, 1);
                dateCell.Value = $"Date Range   :  {model.InspectionDateFrom:dd MMM yyyy}  To  {model.InspectionDateTo:dd MMM yyyy}";
                dateCell.Style.Font.Bold = true;
                dateCell.Style.Font.FontSize = 10;
                worksheet.Range(4, 1, 4, totalColumns).Merge();

                // --- TABEL HEADER (Hanya 3 Kolom) ---
                int currentRow = 6;
                worksheet.Cell(currentRow, 1).Value = "Course Code";
                worksheet.Cell(currentRow, 2).Value = "Course Name";

                worksheet.Cell(currentRow, 3).Value = "Group Code";
                worksheet.Cell(currentRow, 4).Value = "Group Name";
                worksheet.Cell(currentRow, 5).Value = "Abnormal Frequency";

                var headerRange = worksheet.Range(currentRow, 1, currentRow, totalColumns);
                headerRange.Style.Fill.BackgroundColor = XLColor.LightBlue;
                headerRange.Style.Font.Bold = true;
                headerRange.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;

                currentRow++;

                // --- ISI DATA ---
                if (data != null && data.Any())
                {
                    foreach (var item in data)
                    {
                        worksheet.Cell(currentRow, 1).Value = item.CourseCode;
                        worksheet.Cell(currentRow, 2).Value = item.CourseName;

                        worksheet.Cell(currentRow, 3).Value = item.GroupCode;
                        worksheet.Cell(currentRow, 4).Value = item.GroupName;
                        worksheet.Cell(currentRow, 5).Value = item.AbnormalFrequency;
                        //if (int.TryParse(item.AbnormalFrequency, out int frequency))
                        //    worksheet.Cell(currentRow, 3).Value = frequency;
                        //else
                        //    worksheet.Cell(currentRow, 3).Value = item.AbnormalFrequency;

                        currentRow++;
                    }

                    var dataRange = worksheet.Range(6, 1, currentRow - 1, totalColumns);
                    dataRange.Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
                    dataRange.Style.Border.InsideBorder = XLBorderStyleValues.Thin;
                }

                worksheet.Columns(1, totalColumns).AdjustToContents();

                // --- GENERATE FILE ---
                using (var stream = new MemoryStream())
                {
                    workbook.SaveAs(stream);
                    string fileName = $"Top_Plan_Abnormality_Course_{DateTime.Now:yyyyMMdd_HHmmss}.xlsx";
                    return File(stream.ToArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", fileName);
                }
            }
        }

        [HttpPost("exportinspection")]
        public async Task<IActionResult> ExportInspection([FromBody] TopPlanAbnormalityInspectionParamDto model)
        {
            var data = await _repo.GetDataInspectionAsync(model);

            using (var workbook = new XLWorkbook())
            {
                var worksheet = workbook.Worksheets.Add("Top Inspection Abnormality");
                int totalColumns = 3;

                // --- HEADER ---
                var titleCell = worksheet.Cell(1, 1);
                titleCell.Value = "Top Inspection Abnormality Report";
                titleCell.Style.Font.Bold = true;
                titleCell.Style.Font.FontSize = 16;
                worksheet.Range(1, 1, 1, totalColumns).Merge();

                var filterCell = worksheet.Cell(3, 1);
                string inspectionFilter = string.IsNullOrEmpty(model.InspectionPointCode) ? "ALL" : model.InspectionPointCode;
                filterCell.Value = $"Inspection Filter :  {inspectionFilter}";
                filterCell.Style.Font.Bold = true;
                filterCell.Style.Font.FontSize = 10;
                worksheet.Range(3, 1, 3, totalColumns).Merge();

                var dateCell = worksheet.Cell(4, 1);
                dateCell.Value = $"Date Range   :  {model.InspectionDateFrom:dd MMM yyyy}  To  {model.InspectionDateTo:dd MMM yyyy}";
                dateCell.Style.Font.Bold = true;
                dateCell.Style.Font.FontSize = 10;
                worksheet.Range(4, 1, 4, totalColumns).Merge();

                // --- TABEL HEADER (Hanya 3 Kolom) ---
                int currentRow = 6;
                worksheet.Cell(currentRow, 1).Value = "Group Name";
                worksheet.Cell(currentRow, 2).Value = "Inspection Name";
                worksheet.Cell(currentRow, 3).Value = "Abnormal Frequency";

                var headerRange = worksheet.Range(currentRow, 1, currentRow, totalColumns);
                headerRange.Style.Fill.BackgroundColor = XLColor.LightBlue;
                headerRange.Style.Font.Bold = true;
                headerRange.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;

                currentRow++;

                // --- ISI DATA ---
                if (data != null && data.Any())
                {
                    foreach (var item in data)
                    {
                        worksheet.Cell(currentRow, 1).Value = item.GroupName;
                        worksheet.Cell(currentRow, 2).Value = item.InspectionName;
                        worksheet.Cell(currentRow, 3).Value = item.AbnormalFrequency;

                        currentRow++;
                    }

                    var dataRange = worksheet.Range(6, 1, currentRow - 1, totalColumns);
                    dataRange.Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
                    dataRange.Style.Border.InsideBorder = XLBorderStyleValues.Thin;
                }

                worksheet.Columns(1, totalColumns).AdjustToContents();

                // --- GENERATE FILE ---
                using (var stream = new MemoryStream())
                {
                    workbook.SaveAs(stream);
                    string fileName = $"Top_Plan_Abnormality_Inspection_{DateTime.Now:yyyyMMdd_HHmmss}.xlsx";
                    return File(stream.ToArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", fileName);
                }
            }
        }
    }
}