using Microsoft.AspNetCore.Mvc;
using NSI.Domain.Models.Auth;
using System.Net;
using System.ServiceModel.Channels;

namespace NSI.Application.Extensions
{
    public static class ServicesResponseExtensions
    {
       public static IActionResult ResponseStatus<T>(this Controller controller, ApiResponse<T> result)
       {
            var response = new
            {
                success = result.Status,
                message = result.Message,
                data = result.Data,
                code = result.Code
            };

            return result.StatusCode switch
            {
                HttpStatusCode.OK =>
                    controller.Ok(response),

                HttpStatusCode.BadRequest =>
                    controller.BadRequest(response),

                HttpStatusCode.Unauthorized =>
                    controller.Unauthorized(response),

                HttpStatusCode.Forbidden =>
                    controller.StatusCode(StatusCodes.Status403Forbidden, response),

                HttpStatusCode.NotFound =>
                    controller.NotFound(response),

                HttpStatusCode.Conflict =>
                    controller.Conflict(response),

                HttpStatusCode.InternalServerError =>
                    controller.StatusCode(StatusCodes.Status500InternalServerError, response),

                _ =>
                    controller.StatusCode((int)result.StatusCode, response)
            };

            //return result.StatusCode switch
            //{
            //    HttpStatusCode.Unauthorized =>
            //        controller.Unauthorized(new { message = result.Message }),
                        
            //    HttpStatusCode.Forbidden =>
            //        controller.StatusCode(403, new { message = result.Message }),

            //    HttpStatusCode.OK =>
            //        controller.StatusCode(200, new { status = result.Status , message = result.Message , data = result.Data}),
            //    _ =>
            //        controller.StatusCode((int)result.StatusCode )
            //};
        }
    }
}
