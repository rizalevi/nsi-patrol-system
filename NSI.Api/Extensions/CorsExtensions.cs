using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace NSI.Api.Extensions
{
    public static class CorsExtensions
    {
        public static IServiceCollection AddCorsExtension(
            this IServiceCollection services, IConfiguration configuration)
        {
            var allowedOrigins = configuration
                .GetSection("CorsSettings:AllowedOrigins")
                .Get<string[]>() ?? Array.Empty<string>();

         
            services.AddCors(options =>
            {
                options.AddPolicy("NsiCorsPolicy", policy =>
                {
                    policy
                        .WithOrigins(allowedOrigins)
                        .AllowAnyHeader()
                        .AllowAnyMethod();
                });
            });

            return services;
        }
    }
}
