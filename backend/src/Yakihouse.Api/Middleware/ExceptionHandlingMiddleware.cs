using System.Net;
using System.Text.Json;
using Yakihouse.Domain.Common.Exceptions;

namespace Yakihouse.Api.Middleware;

/// <summary>
/// Global exception handling middleware
/// Catches all unhandled exceptions and returns appropriate HTTP responses
/// </summary>
public class ExceptionHandlingMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<ExceptionHandlingMiddleware> _logger;
    private readonly IHostEnvironment _environment;

    public ExceptionHandlingMiddleware(
        RequestDelegate next,
        ILogger<ExceptionHandlingMiddleware> logger,
        IHostEnvironment environment)
    {
        _next = next;
        _logger = logger;
        _environment = environment;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (Exception ex)
        {
            await HandleExceptionAsync(context, ex);
        }
    }

    private async Task HandleExceptionAsync(HttpContext context, Exception exception)
    {
        _logger.LogError(exception, "An unhandled exception occurred: {Message}", exception.Message);

        var (statusCode, errorResponse) = exception switch
        {
            EntityNotFoundException notFoundEx => (
                HttpStatusCode.NotFound,
                new ErrorResponse
                {
                    Type = "EntityNotFound",
                    Title = "Entity Not Found",
                    Status = (int)HttpStatusCode.NotFound,
                    Detail = notFoundEx.Message,
                    Instance = context.Request.Path,
                    Errors = new Dictionary<string, string[]>
                    {
                        ["EntityName"] = new[] { notFoundEx.EntityName },
                        ["Id"] = new[] { notFoundEx.Id.ToString() }
                    }
                }),

            BusinessRuleValidationException businessEx => (
                HttpStatusCode.BadRequest,
                new ErrorResponse
                {
                    Type = "BusinessRuleViolation",
                    Title = "Business Rule Violation",
                    Status = (int)HttpStatusCode.BadRequest,
                    Detail = businessEx.Message,
                    Instance = context.Request.Path
                }),

            _ => (
                HttpStatusCode.InternalServerError,
                new ErrorResponse
                {
                    Type = "InternalError",
                    Title = "Internal Server Error",
                    Status = (int)HttpStatusCode.InternalServerError,
                    Detail = _environment.IsDevelopment() 
                        ? exception.ToString() 
                        : "An internal error occurred. Please contact support.",
                    Instance = context.Request.Path
                })
        };

        context.Response.StatusCode = (int)statusCode;
        context.Response.ContentType = "application/problem+json";

        var json = JsonSerializer.Serialize(errorResponse, new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            WriteIndented = _environment.IsDevelopment()
        });

        await context.Response.WriteAsync(json);
    }
}

/// <summary>
/// RFC 7807 Problem Details response
/// </summary>
public class ErrorResponse
{
    public string Type { get; set; } = string.Empty;
    public string Title { get; set; } = string.Empty;
    public int Status { get; set; }
    public string Detail { get; set; } = string.Empty;
    public string Instance { get; set; } = string.Empty;
    public Dictionary<string, string[]>? Errors { get; set; }
}
