using Microsoft.Extensions.DependencyInjection;

using NSI.Application.Services;
using NSI.Domain.Interfaces;

using NSI.Infra.Helpers;
using NSI.Infra.Repositories;

namespace NSI.Application.Extensions
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddApplication(
            this IServiceCollection services)
        {
            // =========================
            // Application Services
            // =========================
            //services.AddScoped<IRefreshTokenService, RefreshTokenService>();
           // services.AddScoped<IMenuRepository, MenuRepository>();
            //services.AddScoped<MenuService>();
            //services.AddScoped<IRefreshTokenService, RefreshTokenService>();
            //services.AddScoped<RateLimitService>();

            //services.AddScoped<IRefreshTokenService, RefreshTokenService>();
            //services.AddHttpContextAccessor();
            //services.AddHttpClient();
            //services.AddScoped<HttpRequestHelper>();


            return services;
        }
    }
}
