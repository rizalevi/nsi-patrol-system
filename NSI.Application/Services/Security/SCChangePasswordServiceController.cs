using Azure;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NSI.Application.Extensions;
using NSI.Domain.DTOs;
using NSI.Domain.DTOs.Master;
using NSI.Domain.DTOs.Security;
using NSI.Domain.Models.Auth;
using NSI.Domain.Models.Master;
using NSI.Domain.Models.Security;
using NSI.Infra.Helpers;
using System.Security.Claims;
using System.Text.Json;

namespace NSI.Application.Services.Security
{


    [ApiController, Route("service/changepassword")]
    public class SCChangePasswordServiceController : Controller
    {
        private readonly HttpRequestHelper _htr;

        public SCChangePasswordServiceController(HttpRequestHelper htr)  
        {
            _htr = htr;
        }


        [Authorize, HttpPost, Route("update-password")]
        public async Task<IActionResult> Save([FromBody] ChangePassword payload)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier) ?? "";
            var result = await _htr.AsyncAsResponseModel<object>(
                JsonSerializer.Serialize(payload),
                "api/changepassword/save",
                HttpMethod.Post
            );
            return this.ResponseStatus(result);
        }
    }

    
}
