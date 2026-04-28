using EcommerceLifestyle.BLL.Exceptions;
using EcommerceLifestyle.BLL.Interfaces;
using EcommerceLifestyle.DAL.Entities;
using System.Security.Claims;

namespace EcommerceLifestyle.Api.Middleware;

// Outermost middleware. Catches everything that bubbles up from controllers/services
// and writes a uniform JSON shape that the React frontend's errorNormalizer expects.
public class ErrorHandlingMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<ErrorHandlingMiddleware> _log;

    public ErrorHandlingMiddleware(RequestDelegate next, ILogger<ErrorHandlingMiddleware> log)
    {
        _next = next;
        _log = log;
    }

    public async Task Invoke(HttpContext ctx, ILogService logs)
    {
        try
        {
            await _next(ctx);
        }
        catch (ApiException apiEx)
        {
            // Domain-thrown errors -- already shaped, just write them out.
            await Write(ctx, apiEx.StatusCode, apiEx.Message, apiEx.Field, apiEx.Code);
        }
        catch (Exception ex)
        {
            _log.LogError(ex, "Unhandled exception at {Path}", ctx.Request.Path);

            // Persist into Logs table -- but never let logging crash the response.
            try
            {
                await logs.WriteAsync(new Log
                {
                    Timestamp = DateTime.UtcNow,
                    Level = "Error",
                    Source = "ErrorHandler",
                    Message = ex.Message,
                    RequestPath = ctx.Request.Path,
                    Exception = ex.ToString(),
                    UserId = TryGetUserId(ctx)
                });
            }
            catch
            {
                // swallowed on purpose
            }

            await Write(ctx, 500, "Internal server error.", null, "INTERNAL");
        }
    }

    private static Task Write(HttpContext ctx, int status, string message, string? field, string? code)
    {
        ctx.Response.StatusCode = status;
        ctx.Response.ContentType = "application/json";
        return ctx.Response.WriteAsJsonAsync(new
        {
            status,
            message,
            field,
            code
        });
    }

    public static int? TryGetUserId(HttpContext ctx)
    {
        var raw = ctx.User?.FindFirstValue(ClaimTypes.NameIdentifier)
                  ?? ctx.User?.FindFirstValue("sub");
        return int.TryParse(raw, out var id) ? id : null;
    }
}
