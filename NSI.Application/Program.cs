
using Microsoft.AspNetCore.Authentication.Cookies;
using NSI.Application.Extensions;
using NSI.Application.Services.Dashboard;
using NSI.Application.Services.Inspection;
using NSI.Application.Services.InspectionResult;
using NSI.Application.Services.Master;
using NSI.Application.Services.Report;
using NSI.Application.Services.Security;
using NSI.Application.Services.Tasklist;
using NSI.Domain.Models.Common;
//using NSI.Domain.Services;
using NSI.Infra.Extensions;
using NSI.Infra.Helpers;
using OfficeOpenXml;

var builder = WebApplication.CreateBuilder(args);

Console.WriteLine("ENV = " + builder.Environment.EnvironmentName);
var appsUrl = new Uri(builder.Configuration["AppSettings:AppsUrl"] ?? "").GetLeftPart(UriPartial.Authority); // Mengambil Api URL (scheme + host + port)
var baseUrl = new Uri(builder.Configuration["AppSettings:BaseUrl"] ?? "").GetLeftPart(UriPartial.Authority);  // Mengambil Base URL (scheme + host + port)

builder.Services.Configure<AppSettingsModel>(builder.Configuration.GetSection("AppSettings"));
//builder.Configuration
//    .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);

builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.PropertyNamingPolicy = null;  // Pertahankan nama properti asli (PascalCase)
        options.JsonSerializerOptions.PropertyNameCaseInsensitive = true;  // Opsional: Case-insensitive deserialisasi
    });
ExcelPackage.License.SetNonCommercialPersonal("TOS2026");

builder.Services.AddControllersWithViews();
//builder.Services.AddHttpClient<IAuthApiService, AuthApiService>("ApiClient", client =>
//{
//    var apiUrl = builder.Configuration["AppSettings:ApiUrl"]
//        ?? throw new Exception("AppSettings:ApiUrl belum diset");

//    client.BaseAddress = new Uri(apiUrl);
//});
builder.Services.AddHttpContextAccessor();
//builder.Services.AddHttpClient<HttpRequestHelper>(client =>
//{
//    client.Timeout = TimeSpan.FromSeconds(30);
//});

builder.Services.AddHttpClient<HttpRequestHelper>(client =>
{
    client.BaseAddress = new Uri(builder.Configuration["AppSettings:ApiUrl"]);
    client.Timeout = TimeSpan.FromSeconds(30);
});

//builder.Services.AddScoped<HttpRequestHelper>();
//builder.Services.AddHttpContextAccessor();
//builder.Services.AddHttpClient();

builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.LoginPath = "/Account/Login";
        options.LogoutPath = "/Account/Logout";
        options.Cookie.HttpOnly = true;
        options.Cookie.SameSite = SameSiteMode.Strict;

        
        options.Events = new CookieAuthenticationEvents
        {
            OnRedirectToLogin = context =>
            {
                context.Response.Redirect("/Account/Login");
                return Task.CompletedTask;
            }
        };
    });

//versi sebelumnya pakai gini
builder.Services.AddCors(options =>
{
    options.AddPolicy("NsiCorsPolicy", policy =>
    {
        policy.WithOrigins(appsUrl, baseUrl)
        //policy.WithOrigins(proxyUrl, apiUrl, baseUrl, authUrl, notifUrl, storageUrl)
        .AllowAnyMethod()
            //.WithHeaders("Content-Type", "Authorization", "Cookie")
            .AllowAnyHeader()
            .AllowCredentials(); // Mengizinkan kredensial (cookie, dll.)
    });
});

//var allowedOrigins = builder.Configuration
//    .GetSection("CorsSettings:AllowedOrigins")
//    .Get<string[]>();

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

builder.Services.AddAuthorization();
builder.Services.AddInfrastructure(builder.Configuration); 
builder.Services.AddApplication();
//master
builder.Services.AddScoped<MSClusterServiceController>();
builder.Services.AddScoped<MSPlantServiceController>();
builder.Services.AddScoped<MSCourseServiceController>();
builder.Services.AddScoped<MSEquipmentGroupServiceController>();
builder.Services.AddScoped<MSUnitClassificationServiceController>();
builder.Services.AddScoped<MSInspectionPointServiceController>();
builder.Services.AddScoped<MSShiftServiceController>();
builder.Services.AddScoped<MSDeviceServiceController>();

//inspection data
builder.Services.AddScoped<InspectionDataCheckServiceController>();
builder.Services.AddScoped<InspectionDataApprovalServiceController>();

//report
builder.Services.AddScoped<InspectionDataDDLServiceController>();
builder.Services.AddScoped<InspectionDataUploadServiceController>();
builder.Services.AddScoped<InspectionDataInquiryServiceController>();
builder.Services.AddScoped<InspectionResultByItemServiceController>();
builder.Services.AddScoped<MSTraceLogServiceController>();
builder.Services.AddScoped<TopPlanAbnormalityServicesController>();

//dashboard
builder.Services.AddScoped<DashboardServiceController>();

//tasklist
builder.Services.AddScoped<TasklistServiceController>();

//security setting
builder.Services.AddScoped<SCUserSetupServiceController>();
builder.Services.AddScoped<SCGroupSetupServiceController>();
builder.Services.AddScoped<SCChangePasswordServiceController>();

//notif
builder.Services.AddScoped<NotificationServiceController>();
builder.Services.AddScoped<ErrorLoggerServiceController>();


var app = builder.Build();
if (app.Environment.IsDevelopment())
{
   // app.UseDeveloperExceptionPage();
    app.UseExceptionHandler("/Error/ServerError");
}
else
{
    app.UseExceptionHandler("/Error/ServerError");
}

app.UseStatusCodePages(async context =>
{
    var response = context.HttpContext.Response;

    //if (response.StatusCode == 404)
    //    response.Redirect("/Error/NotFoundPage");

    if (response.StatusCode == 403)
        response.Redirect("/Error/NoAccess");
});

app.UseHttpsRedirection();

// Middleware CORS - Harus ditempatkan sebelum UseRouting
app.UseCors("NsiCorsPolicy");


app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();
//app.MapDefaultControllerRoute();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Account}/{action=Login}/{id?}"
);
//app.MapControllerRoute(
//    name: "default",
//    pattern: "{controller=Auth}/{action=Login}/{id?}");
app.UseStaticFiles(); // Wajib agar folder wwwroot bisa diakses langsung
app.Run();
