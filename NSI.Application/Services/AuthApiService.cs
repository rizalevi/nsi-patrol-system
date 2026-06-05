using Microsoft.AspNetCore.Identity.Data;
using System.Net.Http.Json;
using NSI.Domain.Models.Auth;
using NSI.Application.Services;

namespace NSI.Application.Services
{

    public class AuthApiService : IAuthApiService
    {
        private readonly HttpClient _http;

        public AuthApiService(HttpClient http)
        {
            _http = http;
        }

        public async Task<LoginResponse?> LoginAsync(string userId, string password)
        {
            var response = await _http.PostAsJsonAsync(
                "/api/auth/login",
                new Login
                {
                    UserId = userId,
                    Password = password
                });

            if (!response.IsSuccessStatusCode)
                return null;

            return await response.Content.ReadFromJsonAsync<LoginResponse>();
        }

       
    }
}
