using FluentValidation;
using Newtonsoft.Json;
using System.Net;

namespace Product.Controller.Middleware;

public class ExceptionHandlingMiddleware(RequestDelegate next)
{
    private readonly RequestDelegate _next = next;

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (ValidationException ex)
        {
            await HandleExceptionAsync(context, ex, HttpStatusCode.BadRequest);
        }
        catch (Exception ex)
        {
            await HandleExceptionAsync(context, ex, HttpStatusCode.InternalServerError);
        }
    }

    private async Task HandleExceptionAsync(HttpContext context, Exception exception, HttpStatusCode statusCode)
    {
        Console.WriteLine($"Error: {exception.Message}");

        context.Response.StatusCode = (int)statusCode;
        context.Response.ContentType = "application/json";

        var errorResponse = new
        {
            success = false,
            message = exception.Message,
            details = exception.StackTrace
        };

        var errorJson = JsonConvert.SerializeObject(errorResponse);
        await context.Response.WriteAsync(errorJson);
    }
}