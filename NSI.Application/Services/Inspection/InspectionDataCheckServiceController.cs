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


    [ApiController, Route("service/inspectiondatacheck")]
    public class InspectionDataCheckServiceController : Controller
    {
        private readonly HttpRequestHelper _htr;


        public InspectionDataCheckServiceController(HttpRequestHelper htr)
        {
            _htr = htr;
        }

        [Authorize, HttpPost, Route("list")]
        public async Task<IActionResult> GetList([FromBody] InspectionDataLeaderParamDto payload)
        {
            //ResponseModel result = await _htr.AsyncAsResponseModel(JsonSerializer.Serialize(param), "ApiClient", "/api/dailyactivity/getdetailformula");
            var result = await _htr.PostAsyncDataTable<List<InspectionDataLeaderListDto>>(
                 JsonSerializer.Serialize(payload),
                "api/inspectiondatacheck/list"
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
        public async Task<IActionResult> GetDetail([FromBody] LeaderParamDetailDto payload)
        {
            var result = await _htr.PostAsyncDataTable<List<InspectionDataLeaderListDetailDto>>(
                 JsonSerializer.Serialize(payload),
                "api/inspectiondatacheck/list-detail"
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
        //public async Task<IActionResult> ApproveBatch([FromBody] InspectionDataLeader payload)
        //{

        //    var userId = User.FindFirstValue(ClaimTypes.NameIdentifier) ?? "";
        //    var result = await _htr.AsyncAsResponseModel<object>(
        //        JsonSerializer.Serialize(payload),
        //        "api/inspectiondatacheck/approve-batch",
        //        HttpMethod.Post
        //    );

        //    //push notification
        //    await _htr.AsyncAsResponseModel<object>(
        //        JsonSerializer.Serialize(payload),
        //        "api/notification/leader-batch",
        //        HttpMethod.Post
        //    );
        //    //await _notification.SendNotifCheckLeaderBatch(payload, userLogin);


        //    return this.ResponseStatus(result);
        //    //return Json(new { success = result.Status, message = result.Message });
        //}

        [Authorize, HttpPost, Route("approve-batch")]
        public async Task<IActionResult> ApproveBatch([FromBody] InspectionDataLeader payload)
        {
            var result = await _htr.AsyncAsResponseModel<ResponseApprove>(
                JsonSerializer.Serialize(payload),
                "api/inspectiondatacheck/approve-batch",
                HttpMethod.Post
            );

            // push realtime hanya jika notif berhasil dibuat
            if (result != null &&
                result.Status &&
                result.Data != null &&
                result.Data.NeedPushNotification)
            {
                try
                {
                    //push notification
                    await _htr.AsyncStringList<string>(
                        result.Data.TargetUser, //JsonSerializer.Serialize(payload),
                        "api/notification/leader-batch-pushnotif",
                        HttpMethod.Post
                  );
                }
                catch (Exception ex)
                {
                    //_logger.LogError(ex, "SignalR Push Failed");
                }
            }

            return this.ResponseStatus(result);
        }


        [Authorize, HttpPost, Route("approve-detail")]
        public async Task<IActionResult> ApproveDetail([FromBody] InspectionDataLeaderDetail payload)
        {
            var userLogin = User.FindFirstValue(ClaimTypes.NameIdentifier) ?? "";
            var result = await _htr.AsyncAsResponseModel<ResponseApprove>(
                 JsonSerializer.Serialize(payload),
                "api/inspectiondatacheck/approve-detail",
                HttpMethod.Post
            );

            // push realtime hanya jika notif berhasil dibuat
            if (result != null &&
                result.Status &&
                result.Data != null &&
                result.Data.NeedPushNotification)
            {
                try
                {

                    //push notification
                    await _htr.AsyncStringList <string>(
                          result.Data.TargetUser,
                          "api/notification/leader-detail-pushnotif",
                          HttpMethod.Post
                    );
                }
                catch (Exception ex)
                {
                    //_logger.LogError(ex, "SignalR Push Failed");
                }
            }

            ////push notification
            //await _htr.AsyncAsResponseModel<object>(
            //    JsonSerializer.Serialize(payload),
            //    "api/notification/leader-submit",
            //    HttpMethod.Post
            //);

            return this.ResponseStatus(result);
            //return Json(new { success = result.Status, message = result.Message });
        }
    }
}