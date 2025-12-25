using System.Net;
using System.Text.Json;
using MongoCrudApi.Responses;

namespace MongoCrudApi.Middlewares;

// This middleware catches ALL unhandled exceptions
public class ExceptionMiddleware
{
    private readonly RequestDelegate _next;

    public ExceptionMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            // Pass request to next middleware / controller
            await _next(context);
        }
        catch (Exception ex)
        {
            // If any exception occurs, handle it here
            await HandleExceptionAsync(context, ex);
        }
    }

    private static Task HandleExceptionAsync(HttpContext context, Exception exception)
    {
        // Set HTTP status code to 500
        context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;

        // Tell client response is JSON
        context.Response.ContentType = "application/json";

        // Create consistent error response
        var response = new ApiResponse
        {
            Success = false,
            Message = "Something went wrong. Please try again later."
        };

        // Convert C# object to JSON
        var json = JsonSerializer.Serialize(response);

        return context.Response.WriteAsync(json);
    }
}
