using NSI.Api.Extensions;
using NSI.Infra.Extensions;
//using NSI.Application.Extensions;
using Serilog;
using AspNetCore.Scalar;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using NSI.Infra.Helpers;
using NSI.Api.Handlers;
using NSI.Api.Services.Master;
using NSI.Api.Middlewares;
using NSI.Api.Services.Security;
using DocumentFormat.OpenXml.Office2016.Drawing.ChartDrawing;
using NSI.Api.Auth;
using NSI.Domain.Models.Common;
using NSI.Api.Hubs;
using NSI.Api.Services;
using Microsoft.AspNetCore.Diagnostics;
using NSI.Domain.Models.Logger;
using System.Security.Claims;
using NSI.Infra.Repositories.Logger;
using NSI.Domain.Interfaces.Logger;
using NSI.Api.Services.Mobile;
using NSI.Api.Repositories.Mobile;
using NSI.Domain.Interfaces.Mobile;
using Microsoft.AspNetCore.HttpOverrides;

var builder = WebApplication.CreateBuilder(args);
var apiUrl = new Uri(builder.Configuration["AppSettings:ApiUrl"] ?? "").GetLeftPart(UriPartial.Authority); // Mengambil Api URL (scheme + host + port)
var baseUrl = new Uri(builder.Configuration["AppSettings:BaseUrl"] ?? "").GetLeftPart(UriPartial.Authority);  // Mengambil Base URL (scheme + host + port)

#region LOGGING
builder.Host.UseSerilog((context, config) =>
{
    config.ReadFrom.Configuration(context.Configuration);
});
#endregion

#region SERVICES
builder.Services
    .AddControllersWithViews()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.PropertyNamingPolicy = null;
    });
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();

//builder.Services.AddSwaggerGen();  //versi bawaan
builder.Services.AddSwaggerExtension(); // untuk open.api extension api
//builder.Services.AddCorsExtension(builder.Configuration); // untuk cors extension api
//builder.Services.AddJwtAuthentication(builder.Configuration); //jwt aut ditaro di extension infrastructure


//var allowedOrigins = builder.Configuration
//        .GetSection("CorsSettings:AllowedOrigins")
//        .Get<string[]>();

//builder.Services.AddCors(options =>
//{
//    options.AddPolicy("NsiCorsPolicy", policy =>
//    {
//        policy.WithOrigins(allowedOrigins)
//              .AllowAnyMethod()
//              .AllowAnyHeader()
//              .AllowCredentials();
//    });
//});

//pake versi sebelumnya gini
builder.Services.AddCors(options =>
{
    options.AddPolicy("NsiCorsPolicy", policy =>
    {
        policy.WithOrigins(apiUrl, baseUrl)
        //policy.WithOrigins(proxyUrl, apiUrl, baseUrl, authUrl, notifUrl, storageUrl)
        .AllowAnyMethod()
            //.WithHeaders("Content-Type", "Authorization", "Cookie")
            .AllowAnyHeader()
            .AllowCredentials(); // Mengizinkan kredensial (cookie, dll.)
    });
});

// =========================
// AUTH & REFRESH TOKEN
// =========================
//builder.Services.AddApplication();
builder.Services.AddInfrastructure(builder.Configuration);
builder.Services.AddScoped<MSClusterRepositories>();
builder.Services.AddScoped<MSPlantRepositories>();
builder.Services.AddScoped<UserSetupRepositories>();
builder.Services.AddScoped<GroupSetupRepositories>();
builder.Services.AddScoped<MSCourseRepositories>();
builder.Services.AddScoped<MSEquipmentGroupRepositories>();
builder.Services.AddScoped<MSUnitClassificationRepositories>();
//builder.Services.AddScoped<MSInspectionPointRepositories>();
//builder.Services.AddScoped<MSShiftRepositories>();
//builder.Services.AddScoped<MSHTRepositories>();

builder.Services.AddScoped<RateLimitService>();
//var sp = builder.Services.BuildServiceProvider();
//sp.GetRequiredService<NSI.Application.Auth.RefreshTokenService>();
builder.Services
    .AddAuthentication("Bearer")
    .AddJwtBearer("Bearer", options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,

            ValidIssuer = builder.Configuration["JwtSettings:Issuer"],
            ValidAudience = builder.Configuration["JwtSettings:Audience"],

            IssuerSigningKey = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(builder.Configuration["JwtSettings:Key"])
            ),

            ClockSkew = TimeSpan.Zero
        };

        // 🔥 AMBIL TOKEN DARI COOKIE
        options.Events = new JwtBearerEvents
        {
            OnMessageReceived = context =>
            {
                var cookieName =
                    builder.Configuration["AppSettings:ApplicationID"]
                    + "AccessToken";

                if (context.Request.Cookies.ContainsKey(cookieName))
                {
                    context.Token = context.Request.Cookies[cookieName];
                }

                return Task.CompletedTask;
            },
            OnChallenge = context =>
            {
                context.HandleResponse();
                context.Response.StatusCode = 401;
                return context.Response.WriteAsync("Unauthorized");
            }
        };


    });

builder.Services.Configure<AppSettingsModel>(
    builder.Configuration.GetSection("AppSettings"));

builder.Services.Configure<ForwardedHeadersOptions>(options =>
{
    options.ForwardedHeaders =
        ForwardedHeaders.XForwardedFor |
        ForwardedHeaders.XForwardedProto;

    options.KnownNetworks.Clear();
    options.KnownProxies.Clear();
});

//builder.Services.AddHttpClient<HttpRequestHelper>(client =>
//{
//    client.Timeout = TimeSpan.FromSeconds(30);
//});

builder.Services.AddHttpClient<HttpRequestHelper>(client =>
{
    client.BaseAddress = new Uri(builder.Configuration["AppSettings:ApiUrl"]);
    client.Timeout = TimeSpan.FromSeconds(30);
}).AddHttpMessageHandler<AuthHandler>();


builder.Services.AddAuthorization();
builder.Services.AddSignalR();
builder.Services.AddScoped<NotificationService>();

//mobile
builder.Services.AddScoped<MobileCheckConnectionRepositories>();
builder.Services.AddScoped<MobileInspectionExportRepositories>();
builder.Services.AddScoped<MobileLastVersionRepositories>();
builder.Services.AddScoped<MobileSyncRepositories>();
builder.Services.AddScoped<MSMobileRepositories>();



#endregion

var app = builder.Build();

#region MIDDLEWARE

//if (app.Environment.IsDevelopment())
//{
    app.UseSwagger();
    app.UseSwaggerUI();
    //open api scalar 
    app.UseScalar(options =>
    {
        options.UseTheme(Theme.Default);
    });

//}
app.UseSerilogRequestLogging();
app.UseMiddleware<ApiExceptionMiddleware>();
app.UseHttpsRedirection();

app.UseCors("NsiCorsPolicy");

app.UseForwardedHeaders();
app.UseAuthentication();
app.UseAuthorization();
app.MapHub<NotificationHub>("/notificationHub");

app.UseExceptionHandler(errorApp =>
{
    errorApp.Run(async context =>
    {
        var feature = context.Features.Get<IExceptionHandlerPathFeature>();
        var exception = feature?.Error;

        var userId = context.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value;

        var log = new ErrorLogModel
        {
            Message = exception?.Message,
            StackTrace = exception?.StackTrace,
            Path = feature?.Path,
            Method = context.Request.Method,
            UserId = userId,
            QueryString = context.Request.QueryString.ToString(),
            RequestId = context.TraceIdentifier
        };
        var logger = context.RequestServices.GetRequiredService<IErrorLogRepository>();
        await logger.LogAsync(log);


        //// ambil service dari DI
        //var logger = context.RequestServices.GetRequiredService<ErrorLogRepository>();
        //await logger.LogAsync(log);

        // redirect ke halaman error
        context.Response.Redirect("/Error/ServerError");
    });
});

app.MapControllers();

#endregion

app.Run();
