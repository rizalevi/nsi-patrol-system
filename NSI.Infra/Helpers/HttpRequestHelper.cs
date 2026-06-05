using System.Net;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using NSI.Domain.Models.Auth;
using static NSI.Infra.Helpers.HttpRequestHelper;

namespace NSI.Infra.Helpers
{
        
    public class HttpRequestHelper
    {
        private readonly HttpClient _httpClient;
        private readonly IHttpContextAccessor _httpContext;
        private readonly IConfiguration _config;


        public HttpRequestHelper(HttpClient httpClient, IHttpContextAccessor httpContext,
        IConfiguration config)
        {
            _httpClient = httpClient;
            _httpContext = httpContext;
            _config = config;

            var apiUrl = _config["AppSettings:ApiUrl"];

            if (string.IsNullOrWhiteSpace(apiUrl))
                throw new Exception("ApiUrl not configured in AppSettings!");

            // 🔥 FIX UTAMA: BaseAddress
            _httpClient.BaseAddress = new Uri(apiUrl.TrimEnd('/') + "/");
        }

        //private string ApiUrl => (_config["AppSettings:ApiUrl"] ?? "").TrimEnd('/');

        //private string BuildUrl(string endpoint)
        //{
        //    if (string.IsNullOrEmpty(ApiUrl))
        //        throw new Exception("ApiUrl not configured!");

        //    endpoint = endpoint?.TrimStart('/') ?? "";

        //    return $"{ApiUrl}/{endpoint}";
        //}

        private static readonly JsonSerializerOptions _jsonOptions =
            new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };

        private void AttachBearerToken(HttpRequestMessage request)
        {
            var cookieName =
                _config["AppSettings:ApplicationID"] + "AccessToken";

            var token = _httpContext.HttpContext?
                .Request
                .Cookies[cookieName];   

            if (!string.IsNullOrWhiteSpace(token))
            {
                request.Headers.Authorization =
                    new System.Net.Http.Headers.AuthenticationHeaderValue(
                        "Bearer", token
                    );
            }
        }

        private string BuildEndpoint(string endpoint)
        {
            if (string.IsNullOrWhiteSpace(endpoint))
                throw new ArgumentException("Endpoint is required.");

            return endpoint.StartsWith("/")
                ? endpoint
                : "/" + endpoint;
        }


        // ============================
        //  ResponseModel
        // ============================
        public async Task<ApiResponse<T>> AsyncAsResponseModel<T>(
            string jsonPayload,
            string endPoint,
            HttpMethod http,
            CancellationToken ct = default)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(endPoint))
                    throw new ArgumentException("Endpoint is required.", nameof(endPoint));

                using var request = new HttpRequestMessage(http, BuildEndpoint(endPoint));

                AttachBearerToken(request);

                if (!string.IsNullOrWhiteSpace(jsonPayload))
                {
                    request.Content = new StringContent(
                        jsonPayload,
                        Encoding.UTF8,
                        "application/json");
                }

                using var response = await _httpClient.SendAsync(request, ct);
                var jsonResult = await response.Content.ReadAsStringAsync(ct);

                var options = new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                };

                ApiResponse<T>? result = null;

                try
                {
                    result = JsonSerializer.Deserialize<ApiResponse<T>>(jsonResult, options);
                }
                catch
                {
                    // fallback jika response bukan format ApiResponse<T>
                }

                result ??= new ApiResponse<T>();

                result.StatusCode = response.StatusCode;
                //result.RawContent = jsonResult;

                if (!response.IsSuccessStatusCode && string.IsNullOrWhiteSpace(result.Message))
                {
                    result.Message = response.ReasonPhrase ?? "Request failed.";
                }

                return result;

                //if (!response.IsSuccessStatusCode)
                //{
                //    return new ApiResponse<T>
                //    {
                //        Status = false,
                //        Message = string.IsNullOrWhiteSpace(jsonResult)
                //            ? response.ReasonPhrase ?? "Request failed."
                //            : jsonResult,
                //        StatusCode = response.StatusCode,
                //        Data = default
                //    };
                //}

                //var result = JsonSerializer.Deserialize<ApiResponse<T>>(
                //    jsonResult,
                //    _jsonOptions) ?? new ApiResponse<T>();

                //result.StatusCode = response.StatusCode;
                //return result;
            }
            catch (OperationCanceledException)
            {
                return new ApiResponse<T>
                {
                    Status = false,
                    Message = "The request was canceled.",
                    StatusCode = (HttpStatusCode)499,
                    Data = default
                };
            }
            catch (Exception ex)
            {
                return new ApiResponse<T>
                {
                    Status = false,
                    Message = ex.Message,
                    StatusCode = HttpStatusCode.InternalServerError,
                    Data = default
                };
            }
        }


        //public async Task<ResponseModel> AsyncAsResponseModel(
        //    string jsonPayload,
        //    string endPoint,
        //    HttpMethod http,
        //    CancellationToken ct = default)
        //{
        //    try
        //    {
        //        if (string.IsNullOrWhiteSpace(endPoint))
        //            throw new ArgumentException("Endpoint is required.", nameof(endPoint));

        //        using var request = new HttpRequestMessage(http, BuildEndpoint(endPoint));

        //        AttachBearerToken(request);

        //        if (!string.IsNullOrWhiteSpace(jsonPayload))
        //        {
        //            request.Content = new StringContent(
        //                jsonPayload,
        //                Encoding.UTF8,
        //                "application/json");
        //        }

        //        using var response = await _httpClient.SendAsync(request, ct);
        //        var jsonResult = await response.Content.ReadAsStringAsync(ct);

        //        if (!response.IsSuccessStatusCode)
        //        {
        //            return new ResponseModel
        //            {
        //                Status = false,
        //                Message = string.IsNullOrWhiteSpace(jsonResult)
        //                    ? response.ReasonPhrase ?? "Request failed."
        //                    : jsonResult,
        //                Code = ((int)response.StatusCode).ToString(),
        //                StatusCode = response.StatusCode,
        //                Content = jsonResult
        //            };
        //        }

        //        var result = JsonSerializer.Deserialize<ResponseModel>(
        //            jsonResult,
        //            _jsonOptions) ?? new ResponseModel();

        //        result.StatusCode = response.StatusCode;
        //        result.Code ??= ((int)response.StatusCode).ToString();

        //        return result;
        //    }
        //    catch (OperationCanceledException)
        //    {
        //        return new ResponseModel
        //        {
        //            Status = false,
        //            Message = "The request was canceled.",
        //            Code = "499",
        //            StatusCode = (HttpStatusCode)499
        //        };
        //    }
        //    catch (Exception ex)
        //    {
        //        return new ResponseModel
        //        {
        //            Status = false,
        //            Message = ex.Message,
        //            Code = "500",
        //            StatusCode = HttpStatusCode.InternalServerError,
        //            Content = ex.ToString()
        //        };
        //    }
        //}

        //public async Task<ResponseModel> AsyncAsResponseModelOld(
        //    string jsonPayload,
        //    string endPoint,
        //    HttpMethod http ,
        //    CancellationToken ct = default)
        //{
        //    try
        //    {
        //        if (string.IsNullOrWhiteSpace(endPoint))
        //            throw new ArgumentException("Endpoint is required.");

        //        //using var request = new HttpRequestMessage(http, endPoint);

        //        // 🔐 ATTACH JWT TOKEN


        //        using (var request = new HttpRequestMessage(http , BuildEndpoint(endPoint)))
        //        {
        //            AttachBearerToken(request);

        //            if (!string.IsNullOrEmpty(jsonPayload))
        //            {
        //                request.Content = new StringContent(jsonPayload, Encoding.UTF8, "application/json");
        //            }
        //            else
        //            {
        //                request.Content = null;
        //            }

        //            var response = await _httpClient.SendAsync(request, ct);
        //            ResponseModel result = new ResponseModel();

        //            if (!response.IsSuccessStatusCode)
        //            {
        //                result = new ResponseModel { Status = false, Message = $"Status Code: {response.StatusCode}" };
        //            }

        //            var jsonResult = await response.Content.ReadAsStringAsync(ct);
        //            var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };

        //             result = JsonSerializer.Deserialize<ResponseModel>(jsonResult, options) ?? new ResponseModel();
        //            return result;
        //        }



        //        //var result = JsonSerializer.Deserialize<ResponseModel>(jsonResult, _jsonOptions) ?? new ResponseModel(); 

        //        //return result;
        //    }
        //    catch (Exception ex)
        //    {
        //        return new ResponseModel
        //        {
        //            Status = false,
        //            Message = ex.Message,
        //            Content = null
        //        };
        //    }
        //}

        public async Task<ApiResponse<T>> PostAsyncDataTable<T>(
            string payload,
            string endPoint,
            CancellationToken ct = default) 
        {
            try
            {
                if (string.IsNullOrWhiteSpace(endPoint))
                    throw new ArgumentException("Endpoint is required.");

                using var request = new HttpRequestMessage(HttpMethod.Post, BuildEndpoint(endPoint));

                AttachBearerToken(request);

                if (!string.IsNullOrWhiteSpace(payload))
                {

                    request.Content = new StringContent(
                        payload,
                        Encoding.UTF8,
                        "application/json"
                    );
                }

                var response = await _httpClient.SendAsync(request, ct);

                if (response.StatusCode == HttpStatusCode.Unauthorized)
                {

                    return new ApiResponse<T>
                    {
                        Status = false,
                        Message = "Unauthorized",
                        StatusCode = HttpStatusCode.Unauthorized
                    };
                }

                if (response.StatusCode == HttpStatusCode.Forbidden)
                {
                    return new ApiResponse<T>
                    {
                        Status = false,
                        Message = "Forbidden",
                        StatusCode = HttpStatusCode.Forbidden
                    };
                }


                var jsonResult = await response.Content.ReadAsStringAsync(ct);

                if (!response.IsSuccessStatusCode)
                {
                    return new ApiResponse<T>
                    {
                        Status = false,
                        Message = jsonResult
                    };
                }

                return JsonSerializer.Deserialize<ApiResponse<T>>(jsonResult, _jsonOptions)
                       ?? new ApiResponse<T> { Status = false };
            }
            catch (Exception ex)
            {
                return new ApiResponse<T>
                {
                    Status = false,
                    Message = ex.Message,
                    StatusCode = HttpStatusCode.InternalServerError
                };
            }
        }

        public async Task<ApiResponse<T>> AsyncStringList<T>(
            string value, string endPoint,
            HttpMethod http,
            CancellationToken ct = default
            )
        {
            try
            {
                if (string.IsNullOrWhiteSpace(endPoint))
                    throw new ArgumentException("Endpoint is required.");

                using var request = new HttpRequestMessage(http, BuildEndpoint(endPoint));

                AttachBearerToken(request);

                // 🔥 AUTO HANDLE STRING PAYLOAD
                if (!string.IsNullOrWhiteSpace(value))
                {
                    // pastikan jadi JSON valid string -> "admin"
                    var jsonPayload = JsonSerializer.Serialize(value);

                    request.Content = new StringContent(
                        jsonPayload,
                        Encoding.UTF8,
                        "application/json");
                }


                var response = await _httpClient.SendAsync(request, ct);
                var jsonResult = await response.Content.ReadAsStringAsync(ct);

                if (!response.IsSuccessStatusCode)
                {
                    return new ApiResponse<T>
                    {
                        Status = false,
                        Message = $"HTTP {(int)response.StatusCode}"
                    };
                }

                return JsonSerializer.Deserialize<ApiResponse<T>>(jsonResult, _jsonOptions) ?? new ApiResponse<T> { Status = false };
            }
            catch (Exception ex)
            {
                return new ApiResponse<T>
                {
                    Status = false,
                    Message = ex.Message
                };
            }
        }

        public async Task<byte[]> PostAsyncAsByteArray(
            string jsonPayload,
            string endPoint,
            CancellationToken ct = default)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(endPoint))
                    throw new ArgumentException("Endpoint is required.");

                using var request = new HttpRequestMessage(HttpMethod.Post, endPoint);

                // Otomatis pasang JWT dari Cookie
                AttachBearerToken(request);

                if (!string.IsNullOrEmpty(jsonPayload))
                {
                    request.Content = new StringContent(
                        jsonPayload,
                        Encoding.UTF8,
                        "application/json");
                }

                var response = await _httpClient.SendAsync(request, ct);

                if (response.IsSuccessStatusCode)
                {
                    // Langsung kembalikan file mentah berupa byte array
                    return await response.Content.ReadAsByteArrayAsync(ct);
                }

                return null;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"PostAsyncAsByteArray Error: {ex.Message}");
                return null;
            }
        }

    }

}

//	public class HttpRequestHelper
//	{
//		private readonly IHttpClientFactory _httpClientFactory;

//		public HttpRequestHelper(IHttpClientFactory httpClientFactory)
//		{
//			_httpClientFactory = httpClientFactory;
//		}

//		public async Task<byte[]> GetAsByteArrayAsync(string jsonPayload = "", string baseUrlInstanceName = "ApiClient", string endPoint = "/api/auth/login", CancellationToken ct = default)
//		{
//			try
//			{
//				using (var request = new HttpRequestMessage(HttpMethod.Get, endPoint))
//				{
//					if (!string.IsNullOrEmpty(jsonPayload))
//					{
//						request.Content = new StringContent(jsonPayload, Encoding.UTF8, "application/json");
//					}
//					else
//					{
//						request.Content = null;
//					}

//					var response = await _httpClientFactory.CreateClient(baseUrlInstanceName).SendAsync(request, ct);

//					if (response.IsSuccessStatusCode)
//					{
//						return await response.Content.ReadAsByteArrayAsync();
//					}

//					return new byte[0];
//				}
//			}
//			catch (HttpRequestException httpEx)
//			{
//				Console.WriteLine($"HttpRequestException in GetAsByteArrayAsync: {httpEx.Message}");
//				return new byte[0];
//			}
//			catch (Exception ex)
//			{
//				Console.WriteLine($"GetAsByteArrayAsync : {ex.Message}");
//				return new byte[0];
//			}
//		}

//		public async Task<ResponseModel> AsyncAsResponseModel(string jsonPayload = "", string baseUrlInstanceName = "ApiClient", string endPoint = "/api/auth/login", CancellationToken ct = default)
//		{
//			try
//			{
//				if (string.IsNullOrEmpty(baseUrlInstanceName))
//				{
//					throw new ArgumentException($"Invalid or null in baseUrlInstanceName [{baseUrlInstanceName}] with jsonPayload: {jsonPayload}");
//				}

//				if (string.IsNullOrEmpty(endPoint))
//				{
//					throw new ArgumentException($"Invalid or null in endPoint [{endPoint}] with jsonPayload: {jsonPayload}");
//				}

//				using (var request = new HttpRequestMessage(HttpMethod.Post, endPoint))
//				{
//					if (!string.IsNullOrEmpty(jsonPayload))
//					{
//						request.Content = new StringContent(jsonPayload, Encoding.UTF8, "application/json");
//					}
//					else
//					{
//						request.Content = null;
//					}

//					var response = await _httpClientFactory.CreateClient(baseUrlInstanceName).SendAsync(request, ct); //SETARA: authclientUrl + api/auth/login

//					if (!response.IsSuccessStatusCode)
//					{
//						return new ResponseModel { Status = false, Message = $"Error Response Status Code: {response.StatusCode}" };
//					}

//					var jsonResult = await response.Content.ReadAsStringAsync(ct);
//					ResponseModel result = JsonSerializer.Deserialize<ResponseModel>(jsonResult) ?? new ResponseModel();
//					return result;
//				}
//			}
//			catch (Exception ex)
//			{
//				Console.WriteLine($"AsyncAsResponseModel : {ex.Message}");
//				return new ResponseModel { Status = false, Content = null, Message = ex.Message };
//			}
//		}

//		public async Task<byte[]> PostAsyncAsResultByte(string jsonPayload = "", string baseUrlInstanceName = "ApiClient", string endPoint = "/api/report/downloadexcel", CancellationToken ct = default)
//		{
//			try
//			{
//				if (string.IsNullOrEmpty(baseUrlInstanceName))
//				{
//					throw new ArgumentException($"Invalid or null in baseUrlInstanceName [{baseUrlInstanceName}] with jsonPayload: {jsonPayload}");
//				}

//				if (string.IsNullOrEmpty(endPoint))
//				{
//					throw new ArgumentException($"Invalid or null in endPoint [{endPoint}] with jsonPayload: {jsonPayload}");
//				}

//				using (var request = new HttpRequestMessage(HttpMethod.Post, endPoint))
//				{
//					if (!string.IsNullOrEmpty(jsonPayload))
//					{
//						request.Content = new StringContent(jsonPayload, Encoding.UTF8, "application/json");
//					}
//					else
//					{
//						request.Content = null;
//					}

//					var response = await _httpClientFactory.CreateClient(baseUrlInstanceName).SendAsync(request, ct); //SETARA: authclientUrl + api/auth/login

//					if (!response.IsSuccessStatusCode)
//					{
//						return new byte[0];
//					}

//					return await response.Content.ReadAsByteArrayAsync(ct);
//				}
//			}
//			catch (HttpRequestException httpEx)
//			{
//				// Specific exception handling for HTTP-related errors
//				Console.WriteLine($"HttpRequestException in PostAsyncAsResultByte: {httpEx.Message}");
//				return new byte[0];
//			}
//			catch (Exception ex)
//			{
//				// Catching other general exceptions
//				Console.WriteLine($"PostAsyncAsResultByte : {ex.Message}");
//				return new byte[0];
//			}
//		}

    
//		public async Task<ResponseModel> UploadFileAsync(string filePath, string baseUrlInstanceName = "ApiClient", string endPoint = "/api/upload/upload-file", CancellationToken ct = default)
//		{
//			try
//			{
//				filePath = CryptHelper.AesDecrypt(filePath);

//				if (string.IsNullOrEmpty(baseUrlInstanceName))
//				{
//					throw new ArgumentException($"Invalid or null in baseUrlInstanceName [{baseUrlInstanceName}] with filePath: {filePath}");
//				}

//				if (string.IsNullOrEmpty(endPoint))
//				{
//					throw new ArgumentException($"Invalid or null in endPoint [{endPoint}] with filePath: {filePath}");
//				}

//				if (string.IsNullOrEmpty(filePath) || !File.Exists(filePath))
//				{
//					throw new ArgumentException($"File path is invalid or file does not exist [{filePath}]");
//				}

//				// Baca file sebagai stream
//				var fileStream = new FileStream(filePath, FileMode.Open, FileAccess.Read);
//				var fileName = Path.GetFileName(filePath);

//				using (var request = new HttpRequestMessage(HttpMethod.Post, endPoint))
//				{
//					// Buat content untuk form-data
//					var formData = new MultipartFormDataContent();
//					var fileContent = new StreamContent(fileStream);
//					fileContent.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("application/octet-stream");
//					//formData.Add(fileContent, "file", CleanHelper.GetCleanFileName(fileName));  // "file" adalah key yang diharapkan oleh server
//					formData.Add(fileContent, "file", fileName);  // "file" adalah key yang diharapkan oleh server

//					// Menambahkan form-data ke request
//					request.Content = formData;

//					var response = await _httpClientFactory.CreateClient(baseUrlInstanceName).SendAsync(request, ct);

//					if (!response.IsSuccessStatusCode)
//					{
//						return new ResponseModel { Status = false, Message = $"Error Response Status Code: {response.StatusCode}" };
//					}

//					var jsonResult = await response.Content.ReadAsStringAsync(ct);
//					ResponseModel result = JsonSerializer.Deserialize<ResponseModel>(jsonResult) ?? new ResponseModel();
//					return result;
//				}
//			}
//			catch (Exception ex)
//			{
//				Console.WriteLine($"UploadFileAsync : {ex.Message}");
//				return new ResponseModel { Status = false, Content = null, Message = ex.Message };
//			}
//		}

//		public async Task<ResponseModel> DeleteFileAsync(string encryptedFilePath, string baseUrlInstanceName = "ApiClient", string endPoint = "/api/upload/delete-file", CancellationToken ct = default)
//		{
//			try
//			{
//				if (string.IsNullOrEmpty(baseUrlInstanceName))
//				{
//					throw new ArgumentException($"Invalid or null in baseUrlInstanceName [{baseUrlInstanceName}] with encryptedFilePath: {encryptedFilePath}");
//				}

//				if (string.IsNullOrEmpty(endPoint))
//				{
//					throw new ArgumentException($"Invalid or null in endPoint [{endPoint}] with encryptedFilePath: {encryptedFilePath}"); //aes encrypted
//				}

//				using (var request = new HttpRequestMessage(HttpMethod.Delete, endPoint + $"/{encryptedFilePath}"))
//				{
//					request.Content = null;
//					var response = await _httpClientFactory.CreateClient(baseUrlInstanceName).SendAsync(request, ct); //SETARA: authclientUrl + api/auth/login

//					if (!response.IsSuccessStatusCode)
//					{
//						return new ResponseModel { Status = false, Message = $"Error Response Status Code: {response.StatusCode}" };
//					}

//					var jsonResult = await response.Content.ReadAsStringAsync(ct);
//					ResponseModel result = JsonSerializer.Deserialize<ResponseModel>(jsonResult) ?? new ResponseModel();
//					return result;
//				}
//			}
//			catch (Exception ex)
//			{
//				Console.WriteLine($"DeleteFileAsync : {ex.Message}");
//				return new ResponseModel { Status = false, Message = ex.Message };
//			}
//		}

//		//eof
//	}
//}