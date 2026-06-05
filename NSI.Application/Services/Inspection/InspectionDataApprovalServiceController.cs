using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NSI.Application.Extensions;
using NSI.Domain.DTOs.Inspection;
using NSI.Domain.Models.Inspection;
using NSI.Domain.Models.Tasklist;
using NSI.Infra.Helpers;
using System.Security.Claims;
using System.Text.Json;

namespace NSI.Application.Services.Inspection
{


    [ApiController, Route("service/inspectiondataapproval")]
    public class InspectionDataApprovalServiceController : Controller
    {
        private readonly HttpRequestHelper _htr;

        public InspectionDataApprovalServiceController(HttpRequestHelper htr)
        {
            _htr = htr;
        }



        [Authorize, HttpPost, Route("list")]
        public async Task<IActionResult> GetList([FromBody] InspectionDataApprovalParamDto payload)
        {
            //ResponseModel result = await _htr.AsyncAsResponseModel(JsonSerializer.Serialize(param), "ApiClient", "/api/dailyactivity/getdetailformula");
            var result = await _htr.PostAsyncDataTable<List<InspectionDataApprovalListDto>>(
                 JsonSerializer.Serialize(payload),
                "api/inspectiondataapproval/list"
            );

            return this.ResponseStatus(result);
            //return Json(new
            //{
            //    status = result.Status,
            //    message = result.Message,
            //    data = result.Data
            //});
        }

        [Authorize, HttpPost, Route("list-detail")]
        public async Task<IActionResult> GetDetail([FromBody] ApprovalParamDetailDto payload)
        {
            var result = await _htr.PostAsyncDataTable<List<InspectionDataApprovalListDetailDto>>(
                 JsonSerializer.Serialize(payload),
                "api/inspectiondataapproval/list-detail"
            );
            return this.ResponseStatus(result);
            //return Json(new
            //{
            //    status = result.Status,
            //    message = result.Message,
            //    data = result.Data
            //});

        }

        //[Authorize, HttpPost, Route("approve-batch")]
        //public async Task<IActionResult> ApproveBatch([FromBody] InspectionDataApprove payload)
        //{

        //    var userId = User.FindFirstValue(ClaimTypes.NameIdentifier) ?? "";
        //    var result = await _htr.AsyncAsResponseModel<object>(
        //        JsonSerializer.Serialize(payload),
        //        "api/inspectiondataapproval/approve-batch",
        //        HttpMethod.Post
        //    );

        //    //push notification
        //    await _htr.AsyncAsResponseModel<object>(
        //        JsonSerializer.Serialize(payload),
        //        "api/notification/supervisor-batch",
        //        HttpMethod.Post
        //    );
        //    return this.ResponseStatus(result);
        //    //return Json(new { success = result.Status, message = result.Message });
        //}

        [Authorize, HttpPost, Route("approve-batch")]
        public async Task<IActionResult> ApproveBatch([FromBody] InspectionDataApprove payload)
        {
            var result = await _htr.AsyncAsResponseModel<ResponseApprove>(
                JsonSerializer.Serialize(payload),
                "api/inspectiondataapproval/approve-batch",
                HttpMethod.Post
            );

            // push realtime hanya jika notif berhasil dibuat
            //if (result != null &&
            //    result.Status &&
            //    result.Data != null &&
            //    result.Data.NeedPushNotification)
            //{
            //    try
            //    {
            //        await _htr.AsyncAsResponseModel<object>(
            //            JsonSerializer.Serialize(payload),
            //            "api/notification/supervisor-batch",
            //            HttpMethod.Post
            //        );
            //    }
            //    catch (Exception ex)
            //    {
            //        //_logger.LogError(ex, "SignalR Push Failed");
            //    }
            //}

            return this.ResponseStatus(result);
        }

        [Authorize, HttpPost, Route("approve-detail")]
        public async Task<IActionResult> ApproveDetail([FromBody] InspectionDataApproveDetail payload)
        {
            var result = await _htr.AsyncAsResponseModel<ResponseApprove>(
                JsonSerializer.Serialize(payload),
                "api/inspectiondataapproval/approve-detail",
                HttpMethod.Post
            );

            // push realtime hanya jika notif berhasil dibuat
            //if (result != null &&
            //    result.Status &&
            //    result.Data != null &&
            //    result.Data.NeedPushNotification)
            //{
            //    try
            //    {
            //        await _htr.AsyncAsResponseModel<object>(
            //            JsonSerializer.Serialize(payload),
            //            "api/notification/supervisor-submit",
            //            HttpMethod.Post
            //        );
            //    }
            //    catch (Exception ex)
            //    {
            //        //_logger.LogError(ex, "SignalR Push Failed");
            //    }
            //}

            return this.ResponseStatus(result);
        }

        //[Authorize, HttpPost, Route("approve-detail")]
        //public async Task<IActionResult> ApproveDetail([FromBody] InspectionDataApproveDetail payload)
        //{
        //    var userLogin = User.FindFirstValue(ClaimTypes.NameIdentifier) ?? "";
        //    var result = await _htr.AsyncAsResponseModel<object>(
        //         JsonSerializer.Serialize(payload),
        //        "api/inspectiondataapproval/approve-detail",
        //        HttpMethod.Post
        //    );

        //    //push notification saat approve atau user send back to inspector user
        //    await _htr.AsyncAsResponseModel<object>(
        //        JsonSerializer.Serialize(payload),
        //        "api/notification/supervisor-submit",
        //        HttpMethod.Post
        //    );

        //    return this.ResponseStatus(result);
        //}
    }
}