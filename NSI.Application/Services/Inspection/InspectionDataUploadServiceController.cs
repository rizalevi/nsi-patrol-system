using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NSI.Domain.DTOs.Inspection;
using NSI.Domain.Models.Tasklist;
using NSI.Infra.Helpers;
using System.Security.Claims;
using System.Text.Json;

namespace NSI.Application.Services.Inspection
{

    [ApiController, Route("service/InspectionDataUpload")]
    public class InspectionDataUploadServiceController : Controller
    {
        private readonly HttpRequestHelper _htr;
        private readonly IWebHostEnvironment _env; // TAMBAHKAN INI

        public InspectionDataUploadServiceController(HttpRequestHelper htr, IWebHostEnvironment env)
        {
            _htr = htr;
            _env = env;
        }



        [Authorize, HttpPost, Route("list")]
        public async Task<IActionResult> GetList([FromBody] InspectionDataUploadParamDto payload)
        {
            //ResponseModel result = await _htr.AsyncAsResponseModel(JsonSerializer.Serialize(param), "ApiClient", "/api/dailyactivity/getdetailformula");
          
            var result = await _htr.PostAsyncDataTable<List<InspectionDataUploadListDto>>(
                 JsonSerializer.Serialize(payload),
                "api/InspectionDataUpload/list"
            );

            return Json(new
            {
                status = result.Status,
                message = result.Message,
                data = result.Data
            });
        }

        [Authorize, HttpPost, Route("approve-inspector")]
        public async Task<IActionResult> ApproveInspector([FromBody] InspectionDataUploadParamDto payload)
        {
            var userLogin = User.FindFirstValue(ClaimTypes.NameIdentifier) ?? "";
            var result = await _htr.AsyncAsResponseModel<ResponseApprove>(
                 JsonSerializer.Serialize(payload),
                "api/InspectionDataUpload/approve-inspector",
                HttpMethod.Post
            );

            
            //------------------------------------------------------------
            // PUSH REALTIME HANYA JIKA NOTIF BERHASIL DIBUAT
            //------------------------------------------------------------
            if (result != null &&
                result.Status &&
                result.Data != null &&
                result.Data.NeedPushNotification)
            {
                try
                {
                    // tetap gunakan endpoint notif lama
                    // karena endpoint ini handle SignalR realtime
                    //push notification
                    await _htr.AsyncAsResponseModel<object>(
                        result.Data.TargetUser, //JsonSerializer.Serialize(payload),
                        "api/notification/approve-inspector-pushnotif",
                        HttpMethod.Post
                    );

                }
                catch (Exception ex)
                {
                    //_logger.LogError(ex, "SignalR Push Failed");
                }
            }


            return Json(new { success = result.Status, message = result.Message });
        }

        [Authorize, HttpPost, Route("getremarksbyleader")]
        public async Task<IActionResult> GetRemarksByLeader([FromBody] InspectionDataUploadParamDto payload)
        {
            var result = await _htr.PostAsyncDataTable<List<InspectionDataUploadRemarksDto>>(
                 JsonSerializer.Serialize(payload),
                "api/InspectionDataUpload/getremarksbyleader"
            );

            return Json(new
            {
                status = result.Status,
                message = result.Message,
                data = result.Data
            });
        }

        [Authorize, HttpPost, Route("getlaststatus")]
        public async Task<IActionResult> GetLastStatus([FromBody] InspectionDataUploadParamDto payload)
        { 
            var result = await _htr.PostAsyncDataTable<List<InspectionDataUploadRemarksDto>>(
                 JsonSerializer.Serialize(payload),
                "api/InspectionDataUpload/getlaststatus"
            );

            return Json(new
            {
                status = result.Status,
                message = result.Message,
                data = result.Data
            });
        } 

        [Authorize, HttpPost, Route("checkstatusleader")]
        public async Task<IActionResult> CheckStatusLeader([FromBody] InspectionDataUploadParamDto payload)
        { 
            var result = await _htr.PostAsyncDataTable<InspectionDataUploadRemarksDto>(
                 JsonSerializer.Serialize(payload),
                "api/InspectionDataUpload/checkstatusleader"
            );

            return Json(new
            {
                status = result.Status,
                message = result.Message,
                data = result.Data
            });
        }

        [Authorize, HttpPost, Route("update-detail")]
        public async Task<IActionResult> UpdateDetail([FromForm] InspectionDataUploadUpdateDto payload)
        {
            payload.LastUser = User.FindFirstValue(ClaimTypes.NameIdentifier) ?? "";

            // SIMPAN FISIK GAMBAR KE WWWROOT SERVER
            if (payload.FileImage != null && payload.FileImage.Length > 0)
            {
                // Buat folder wwwroot/Uploads/Inspections jika belum ada
                string uploadsFolder = Path.Combine(_env.WebRootPath, "Uploads", "Inspections");
                if (!Directory.Exists(uploadsFolder)) Directory.CreateDirectory(uploadsFolder);

                // Buat nama file unik (Mencegah nama file kembar tertimpa)
                string uniqueFileName = $"{payload.InspectionNo}_{Guid.NewGuid().ToString().Substring(0, 5)}{Path.GetExtension(payload.FileImage.FileName)}";
                string filePath = Path.Combine(uploadsFolder, uniqueFileName);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await payload.FileImage.CopyToAsync(stream);
                }

                // Simpan Path-nya ke DTO untuk dikirim ke API
                payload.FileImagePath = $"/Uploads/Inspections/{uniqueFileName}";
                payload.FileNameStr = payload.FileImage.FileName;
                payload.FileImageBytes = null; // Kosongkan agar JSON ringan
            }

            //------------------------------------------------------------
            // UPDATE DETAIL
            //------------------------------------------------------------
            var result = await _htr.AsyncAsResponseModel<ResponseApprove>(
                 JsonSerializer.Serialize(payload),
                "api/InspectionDataUpload/update-detail",
                HttpMethod.Post
            );

            //------------------------------------------------------------
            // PUSH REALTIME HANYA JIKA NOTIF BERHASIL DIBUAT
            //------------------------------------------------------------
            if (result != null &&
                result.Status &&
                result.Data != null &&
                result.Data.NeedPushNotification)
            {
                try
                {
                    // tetap gunakan endpoint notif lama
                    // karena endpoint ini handle SignalR realtime
                    await _htr.AsyncStringList<object>(
                        result.Data.TargetUser, //JsonSerializer.Serialize(payload),
                        "api/notification/inspector-submit-detail-pushnotif",
                        HttpMethod.Post
                    );
                }
                catch (Exception ex)
                {
                    //_logger.LogError(ex, "SignalR Push Failed");
                }
            }

            return Json(new
            {
                success = result?.Status ?? false,
                message = result?.Message ?? "Unknown Error"
            });
        }

        [Authorize, HttpGet, Route("get-image")]
        public async Task<IActionResult> GetImage([FromQuery] string code, [FromQuery] string group, [FromQuery] string no, [FromQuery] string result = "")
        {
            var payload = new InspectionDataUploadUpdateDto
            {
                InspectionCode = code,
                GroupCode = group,
                InspectionNo = no,
                InspectionResult = result
            };

            var apiResponse = await _htr.PostAsyncDataTable<InspectionImageDto>(
                 JsonSerializer.Serialize(payload),
                "api/InspectionDataUpload/get-image"
            );

          
            return Json(new
            {
                status = apiResponse.Status,
                message = apiResponse.Message,
                data = apiResponse.Data
            });


            //// Jika berhasil dapat path dari Database (contoh: "/Uploads/Inspections/file.jpg")
            //if (apiResponse.Status && !string.IsNullOrEmpty(apiResponse.Data))
            //{
            //    // Langsung lempar URL tersebut ke Javascript
            //    return Json(new { success = true, url = apiResponse.Data });
            //}

            //return Json(new { success = false, message = "Path tidak ditemukan di database" });
        }

    }
}