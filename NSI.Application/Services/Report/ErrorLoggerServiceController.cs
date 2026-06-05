
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NSI.Application.Extensions;
using NSI.Domain.DTOs.Logger;

using NSI.Infra.Helpers;
namespace NSI.Application.Services.Report
{

    [ApiController, Route("service/errorlog")]
    public class ErrorLoggerServiceController : Controller
    {
        private readonly HttpRequestHelper _htr;

        public ErrorLoggerServiceController(HttpRequestHelper htr)
        {
            _htr = htr;
        }

        [Authorize, HttpPost, Route("list")]
        public async Task<IActionResult> GetList()
        {
            var result = await _htr.PostAsyncDataTable<List<ErrorDto>>(
                "{}",
                "/api/errorlog/list"
            );
            return this.ResponseStatus(result);
        }

       
    }
}
