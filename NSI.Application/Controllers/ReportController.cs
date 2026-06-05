using Microsoft.AspNetCore.Mvc;
using Microsoft.Reporting.NETCore;
using NSI.Domain.DTOs.Inspection;
using NSI.Domain.Interfaces.Inspection;
using NSI.Infra.Helpers;
using System.Text.Json;

public class ReportController : Controller
{

    private readonly HttpRequestHelper _htr;

    public ReportController(HttpRequestHelper htr)
    {
        //  _authApi = authApi;
        _htr = htr;

    }

    /*INSPECTION INQUIRY REPORT*/

    [HttpGet]
    public async Task<IActionResult> InspectionReport(string courseCode, string inspectionDate,string frequency)
    {
        string path;//= Path.Combine(Directory.GetCurrentDirectory(), "Reports", "InspectionReportDailyAll.rdlc");
        byte[] pdf = new byte[0];

        if (frequency.Contains("Daily Shift"))
        {
            path = Path.Combine(Directory.GetCurrentDirectory(), "Reports", "InspectionReportDaily.rdlc");
            LocalReport report = new LocalReport();
            report.ReportPath = path;
            var data = await GetInspectionReportDaily(courseCode, DateTime.Parse(inspectionDate),frequency);

            report.DataSources.Clear();
            report.DataSources.Add(new ReportDataSource("sp_Web_Report_InspectionDataInquiry_Daily", data));

            pdf = report.Render("PDF");
        }
        else
        {
            path = Path.Combine(Directory.GetCurrentDirectory(), "Reports", "InspectionReportAll.rdlc");
            LocalReport report = new LocalReport();
            report.ReportPath = path;

            var data = await GetInspectionReportAll(courseCode, DateTime.Parse(inspectionDate), frequency);

            report.DataSources.Clear();
            report.DataSources.Add(new ReportDataSource("sp_Web_Report_InspectionDataInquiry", data));

            pdf = report.Render("PDF");
        }



        return File(pdf, "application/pdf");
    }

    private async Task<List<InspectionDataDailyReportDto>> GetInspectionReportDaily(string courseCode, DateTime inspectionDate, string frequency)
    {
        var param = new
        {
            CourseCode = courseCode,
            InspectionDateFrom = inspectionDate,
            Frequency = frequency
        };

        var result = await _htr.PostAsyncDataTable<List<InspectionDataDailyReportDto>>(
            JsonSerializer.Serialize(param),
            "api/inspectiondatainquiry/report-daily"
        );

        return result.Data?.ToList() ?? new List<InspectionDataDailyReportDto>();
    }

    private async Task<List<InspectionDataAllReportDto>> GetInspectionReportAll(string courseCode, DateTime inspectionDate, string frequency)
    {
        var param = new
        {
            CourseCode = courseCode,
            InspectionDateFrom = inspectionDate,
            Frequency = frequency
        };

        var result = await _htr.PostAsyncDataTable<List<InspectionDataAllReportDto>>(
            JsonSerializer.Serialize(param),
            "api/inspectiondatainquiry/report-allshift"
        );

        return result.Data?.ToList() ?? new List<InspectionDataAllReportDto>();
    }

    /*END INSPECTION INQUIRY REPORT*/
}