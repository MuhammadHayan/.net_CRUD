
using Scalar.AspNetCore; // <-- 1. Add this at the top
using MongoCrudApi.Models;
using MongoCrudApi.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services
builder.Services.AddControllers(); // Enables Controller support
builder.Services.AddSingleton<UserService>(); // Registers your DB logic

// Add Swagger/OpenAPI (Optional but recommended)
builder.Services.AddOpenApi();

// This binds the "MongoDbSettings" section from JSON to the MongoDbSettings class
builder.Services.Configure<MongoDbSettings>(
    builder.Configuration.GetSection("MongoDbSettings"));

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference(); // <-- 2. Add this line here
}

app.MapControllers(); // Maps the routes in your Controllers folder

app.Run();