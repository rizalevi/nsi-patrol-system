using Azure;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NSI.Application.Extensions;
using NSI.Domain.DTOs.Security;
using NSI.Domain.Models.Security;
using NSI.Infra.Helpers;
using System.Security.Claims;
using System.Text.Json;

namespace NSI.Application.Services.Security
{


    [ApiController, Route("service/usersetup")]
    public class SCUserSetupServiceController : Controller
    {
        private readonly HttpRequestHelper _htr;
        private readonly IWebHostEnvironment _env; // TAMBAHKAN INI

        public SCUserSetupServiceController(HttpRequestHelper htr, IWebHostEnvironment env)  
        {
            _htr = htr;
            _env = env;
        }

        [Authorize, HttpPost, Route("list")]
        public async Task<IActionResult> GetList()
        {
            //ResponseModel result = await _htr.AsyncAsResponseModel(JsonSerializer.Serialize(param), "ApiClient", "/api/dailyactivity/getdetailformula");
            var result = await _htr.PostAsyncDataTable<List<UserSetupDto>>(
                "{}",
                "api/usersetup/list"
            );
            return this.ResponseStatus(result);
            //return Json(new {
            //    status = result.Status,
            //    message = result.Message,
            //    data = result.Data });
        }

        [Authorize, HttpPost, Route("detail")]
        public async Task<IActionResult> GetData([FromBody] UserSetupInfo  payload)
        {
            var result = await _htr.PostAsyncDataTable<UserSetupInfo>(
                JsonSerializer.Serialize(payload),
                "api/usersetup/detail"
            );
            return this.ResponseStatus(result);
            //return Json(new {
            //    status = result.Status,
            //    message = result.Message,
            //    data = result.Data });

        }

        [Authorize, HttpPost, Route("create")]
        public async Task<IActionResult> Create([FromBody] UserSetup payload)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier) ?? "";
            var result = await _htr.AsyncAsResponseModel<object>(
                JsonSerializer.Serialize(payload),
                "api/usersetup/create",
                HttpMethod.Post
            );


            //if (result.Content != null)
            //{
            //    var contentStr = result.Content.ToString();

            //    try
            //    {
            //        var obj = JsonSerializer.Deserialize<ResponseModel>(contentStr);

            //        if (obj != null)
            //            message = obj.Message;
            //    }
            //    catch
            //    {
            //        message = contentStr;
            //    }
            //}
            return this.ResponseStatus(result);
        }

        [Authorize, HttpPatch, Route("update")]
        public async Task<IActionResult> Update([FromBody] UserSetup payload)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier) ?? "";
            var result = await _htr.AsyncAsResponseModel<object>(
                JsonSerializer.Serialize(payload),
                "api/usersetup/update",
                HttpMethod.Patch
            );
            return this.ResponseStatus(result);
        }


        //delete
        [Authorize, HttpPost, Route("delete")]
        public async Task<IActionResult> Delete([FromBody] UserSetupInfo payload)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier) ?? "";
            var result = await _htr.AsyncAsResponseModel<object>(
                JsonSerializer.Serialize(payload),
                "api/usersetup/remove",
                HttpMethod.Post
            );
            return this.ResponseStatus(result);
        }

        [Authorize, HttpPost, Route("list-setting")]
        public async Task<IActionResult> GetListDetail([FromBody] UserSettingCourse model)
        {
            var result = await _htr.PostAsyncDataTable<List<UserSettingCourseDto>>(
               JsonSerializer.Serialize(model),
                "api/usersetup/list-setting-course"
            );
            return this.ResponseStatus(result);
            //return Json(new {
            //    status = result.Status,
            //    message = result.Message,
            //    data = result.Data });
        }


        [Authorize, HttpPost, Route("submit-batch")]
        public async Task<IActionResult> SubmitBatch([FromBody] List<UserSettingCourse> model)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier) ?? "";
            var result = await _htr.AsyncAsResponseModel<object>(
                JsonSerializer.Serialize(model),
                "api/usersetup/submit-setting-course",
                HttpMethod.Post
            );
            return this.ResponseStatus(result);

        }

        [Authorize, HttpPost, Route("get-profile")]
        public async Task<IActionResult> Profile([FromBody] UserSetupInfo payload)
        {
            var result = await _htr.PostAsyncDataTable<ProfileDto>(
                JsonSerializer.Serialize(payload),
                "api/usersetup/get-profile"
            );
            return this.ResponseStatus(result);

        }

        [Authorize, HttpPatch, Route("save-profile")]
        public async Task<IActionResult> SaveProfile([FromForm] UserSetup payload)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier) ?? "";

            // SIMPAN FISIK GAMBAR KE WWWROOT SERVER
            if (payload.PictureFile != null && payload.PictureFile.Length > 0)
            {
                // Buat folder wwwroot/Uploads/Inspections jika belum ada
                string uploadsFolder = Path.Combine(_env.WebRootPath, "userpicture");
                if (!Directory.Exists(uploadsFolder)) Directory.CreateDirectory(uploadsFolder);

                // Buat nama file unik (Mencegah nama file kembar tertimpa)
                string originalFileName =Path.GetFileNameWithoutExtension(payload.PictureFile.FileName);

                string extension =  Path.GetExtension(payload.PictureFile.FileName);

                string timestamp =  DateTime.Now.ToString("yyyyMMdd_HHmmss");

                string uniqueFileName =  $"{timestamp}_{originalFileName}{extension}";
                string filePath = Path.Combine(uploadsFolder, uniqueFileName);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await payload.PictureFile.CopyToAsync(stream);
                }

                // Simpan Path-nya ke DTO untuk dikirim ke API
                payload.PicturePath = $"/userpicture/{uniqueFileName}";
                //payload.PictureName = originalFileName;
                payload.FileImageBytes = null; // Kosongkan agar JSON ringan
            }

            var result = await _htr.AsyncAsResponseModel<object>(
                JsonSerializer.Serialize(payload),
                "api/usersetup/save-profile",
                HttpMethod.Patch
            );
            return this.ResponseStatus(result);
        }
        //[Authorize, HttpPost, Route("get-picture")]
        //public async Task<IActionResult> GetPicture([FromBody] UserSetupInfo payload)
        //{
        //    var result = await _htr.PostAsyncDataTable<ProfileDto>(
        //        JsonSerializer.Serialize(payload),
        //        "api/usersetup/get-picture"
        //    );
        //    return this.ResponseStatus(result);

        //}
    }
}
