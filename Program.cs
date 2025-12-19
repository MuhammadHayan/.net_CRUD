
using Scalar.AspNetCore;
using MongoCrudApi.Models;
using MongoCrudApi.Services;

var builder = WebApplication.CreateBuilder(args);

// Tell .NET to listen on this port
builder.WebHost.UseUrls("http://*:5160");

// Enable controllers
builder.Services.AddControllers();

// Register Mongo service
builder.Services.AddSingleton<UserService>();

// Bind MongoDbSettings from appsettings.json
builder.Services.Configure<MongoDbSettings>(
    builder.Configuration.GetSection("MongoDbSettings"));

// Add Swagger/OpenAPI (Optional but recommended)
builder.Services.AddOpenApi();

// CORS (⚠️ restrict in production)
builder.Services.AddCors(options => {
    options.AddDefaultPolicy(
        policy => policy.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader()
        );
});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference(); 
}

// Must be before MapControllers
app.UseCors();

// Maps the routes in Controllers folder
app.MapControllers(); 

app.Run();