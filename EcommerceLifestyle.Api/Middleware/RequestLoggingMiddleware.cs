using System.Diagnostics;
using EcommerceLifestyle.BLL.Interfaces;
using EcommerceLifestyle.DAL.Entities;

namespace EcommerceLifestyle.Api.Middleware;

// Records one Logs row per non-swagger HTTP request, after the request has
// been handled. Sits AFTER UseAuthentication so the user-id claim is available.
public class RequestLoggingMiddleware
{
    private readonly RequestDelegate _next;

    public RequestLoggingMiddleware(RequestDelegate next) => _next = next;

    public async Task Invoke(HttpContext ctx, ILogService logs)
    {
        var sw = Stopwatch.StartNew();
        await _next(ctx);
        sw.Stop();

        // Skip noisy paths.
        if (ctx.Request.Path.StartsWithSegments("/swagger")) return;

        try
        {
            var status = ctx.Response.StatusCode;
            await logs.WriteAsync(new Log
            {
                Timestamp = DateTime.UtcNow,
                Level = status >= 500 ? "Error" : status >= 400 ? "Warn" : "Info",
                Source = "RequestLogging",
                Message = $"{ctx.Request.Method} {ctx.Request.Path} -> {status} ({sw.ElapsedMilliseconds}ms)",
                UserId = ErrorHandlingMiddleware.TryGetUserId(ctx),
                RequestPath = ctx.Request.Path
            });
        }
        catch
        {
            // intentionally swallow -- logging must never break the response
        }
    }
}
