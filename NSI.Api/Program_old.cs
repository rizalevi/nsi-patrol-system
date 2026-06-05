
using AspNetCore.Scalar;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using NSI.Api.Services;
using NSI.Domain.Interfaces;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// ================= SERVICES =================

//var allowedOrigins = builder.Configuration
//    .GetSection("CorsSettings:AllowedOrigins")
//    .Get<string[]>();

//builder.Services.AddCors(options =>
//{
//    options.AddPolicy("NsiCorsPolicy", policy =>
//    {
//        policy
//            .WithOrigins(allowedOrigins!)
//            .AllowAnyHeader()
//            .AllowAnyMethod();
//    });
//});

var allowedOrigins = builder.Configuration
    .GetSection("CorsSettings:AllowedOrigins")
    .Get<string[]>() ?? Array.Empty<string>();

builder.Services.AddCors(options =>
{
    options.AddPolicy("NsiCorsPolicy", policy =>
    {
        policy
            .WithOrigins(allowedOrigins)
            .AllowAnyHeader()
            .AllowAnyMethod();
    });
});


// MVC / API Controller
builder.Services.AddControllers();
// 🔐 JWT Service (DOMAIN INTERFACE → API IMPLEMENTATION)
builder.Services.AddScoped<IJwtService, JwtService>();

// 🔎 OPENAPI (WAJIB, SCALAR AMBIL DARI SINI)
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// 🔐 Authentication JWT
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = builder.Configuration["Jwt:Issuer"],
            ValidAudience = builder.Configuration["Jwt:Audience"],

            IssuerSigningKey = new SymmetricSecurityKey(
            Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"])
            )
        };
    });

// ================= BUILD APP =================
var app = builder.Build();
app.UseSwagger();
//open api scalar 
app.UseScalar(options =>
{
    options.UseTheme(Theme.Default);
});


// ================= MIDDLEWARE =================
app.UseCors("NsiCorsPolicy");
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();
app.Run();