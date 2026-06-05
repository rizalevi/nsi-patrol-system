using System.Net.Http.Headers;
using System.Net;
using NSI.Domain.Models.Auth;

namespace NSI.Api.Handlers
{
    public class AuthHandler : DelegatingHandler
    {
        private readonly IHttpContextAccessor _ctx;
        private readonly IConfiguration _config;

        public AuthHandler(IHttpContextAccessor ctx, IConfiguration config)
        {
            _ctx = ctx;
            _config = config;
        }

        protected override async Task<HttpResponseMessage> SendAsync(
            HttpRequestMessage request,
            CancellationToken ct)
        {

            var cookieName =
                _config["AppSettings:ApplicationID"] + "AccessToken";


            var token = _ctx.HttpContext?.Request.Cookies[cookieName];
           

            var response = await base.SendAsync(request, ct);

            if (response.StatusCode == HttpStatusCode.Unauthorized)
            {
                var refreshReq = new HttpRequestMessage(
                    HttpMethod.Post, "/api/auth/refresh");

                var refreshRes = await base.SendAsync(refreshReq, ct);

                if (refreshRes.IsSuccessStatusCode)
                {
                    var retry = CloneRequest(request);
                    var newToken =
                        _ctx.HttpContext?.Request.Cookies[cookieName];

                    retry.Headers.Authorization =
                        new AuthenticationHeaderValue("Bearer", newToken);

                    return await base.SendAsync(retry, ct);
                }
            }

            return response;
        }


        private static HttpRequestMessage CloneRequest(HttpRequestMessage request)
        {
            var clone = new HttpRequestMessage(
                request.Method,
                request.RequestUri
            );

            // Copy headers
            foreach (var header in request.Headers)
                clone.Headers.TryAddWithoutValidation(header.Key, header.Value);

            // Copy content (POST / PUT)
            if (request.Content != null)
            {
                var ms = new MemoryStream();
                request.Content.CopyToAsync(ms).Wait();
                ms.Position = 0;

                clone.Content = new StreamContent(ms);

                foreach (var header in request.Content.Headers)
                    clone.Content.Headers.TryAddWithoutValidation(header.Key, header.Value);
            }

            return clone;
        }
    }

}
