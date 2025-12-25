using Scalar.AspNetCore;
using MongoCrudApi.Models;
using MongoCrudApi.Services;
using MongoCrudApi.Middlewares;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// 🌍 Listen on this port
builder.WebHost.UseUrls("http://*:5160");

// Enable Controllers
builder.Services.AddControllers();

// Register Services
builder.Services.AddSingleton<UserService>();
builder.Services.AddSingleton<JwtService>(); // 🔐 JWT generator

// MongoDB settings
builder.Services.Configure<MongoDbSettings>(
    builder.Configuration.GetSection("MongoDbSettings"));

// 🔐 JWT Authentication
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
                Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]!)
            )
        };
    });

// Swagger
builder.Services.AddOpenApi();

// CORS (⚠️ lock down in production)
builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(policy =>
        policy.AllowAnyOrigin()
              .AllowAnyHeader()
              .AllowAnyMethod());
});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference();
}

// 🧱 Middleware order is CRITICAL

app.UseCors();

// 🔐 Auth must come BEFORE controllers
app.UseAuthentication();
app.UseAuthorization();

// 🌍 Global error handler (catches all crashes)
app.UseMiddleware<ExceptionMiddleware>();

// 🎯 Map Controllers LAST
app.MapControllers();

app.Run();
