using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using EcommerceLifestyle.Api.Middleware;
using EcommerceLifestyle.BLL;
using EcommerceLifestyle.DAL;
using EcommerceLifestyle.DAL.Persistence;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

// ---------- Layered DI ----------
// API knows BLL; BLL knows DAL. The two extension methods below hide all
// the wiring details inside their respective projects.
builder.Services.AddDal(builder.Configuration.GetConnectionString("Default")!);
builder.Services.AddBll();

// ---------- MVC + JSON ----------
builder.Services.AddControllers()
    .AddJsonOptions(o =>
    {
        // camelCase property names match what the React frontend expects.
        o.JsonSerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
        o.JsonSerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull;
    });

// Validation responses go through the same { status, message, field, code } shape.
builder.Services.Configure<ApiBehaviorOptions>(o =>
{
    o.InvalidModelStateResponseFactory = ctx =>
    {
        var first = ctx.ModelState
            .Where(kv => kv.Value!.Errors.Count > 0)
            .Select(kv => new { Field = kv.Key, Msg = kv.Value!.Errors[0].ErrorMessage })
            .FirstOrDefault();

        return new BadRequestObjectResult(new
        {
            status = 400,
            message = first?.Msg ?? "Validation failed.",
            field = first?.Field,
            code = "VALIDATION"
        });
    };
});

// ---------- JWT auth ----------
var jwt = builder.Configuration.GetSection("Jwt");
var keyBytes = Encoding.UTF8.GetBytes(jwt["Key"]!);

builder.Services
    .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(opt =>
    {
        opt.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = jwt["Issuer"],
            ValidAudience = jwt["Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(keyBytes),
            ClockSkew = TimeSpan.FromSeconds(30)
        };
    });

builder.Services.AddAuthorization();

// ---------- CORS ----------
const string CorsPolicy = "FrontendDev";
var allowedOrigins = builder.Configuration
    .GetSection("Cors:AllowedOrigins")
    .Get<string[]>() ?? new[] { "http://localhost:5173", "http://localhost:5174", "http://localhost:4173" };

builder.Services.AddCors(o => o.AddPolicy(CorsPolicy, p =>
    p.SetIsOriginAllowed(origin =>
     {
         if (allowedOrigins.Contains(origin, StringComparer.OrdinalIgnoreCase))
             return true;

         if (Uri.TryCreate(origin, UriKind.Absolute, out var u)
             && u.Scheme == Uri.UriSchemeHttps
             && (u.Host.Equals("vercel.app", StringComparison.OrdinalIgnoreCase)
                 || u.Host.EndsWith(".vercel.app", StringComparison.OrdinalIgnoreCase)))
             return true;

         return false;
     })
     .AllowAnyHeader()
     .AllowAnyMethod()
     .AllowCredentials()));

// ---------- Swagger ----------
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "EcommerceLifestyle API", Version = "v1" });

    // "Authorize" button in Swagger UI -- accepts a Bearer token.
    var scheme = new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Description = "JWT Bearer token (without 'Bearer ' prefix).",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.Http,
        Scheme = "bearer",
        BearerFormat = "JWT",
        Reference = new OpenApiReference { Id = "Bearer", Type = ReferenceType.SecurityScheme }
    };
    c.AddSecurityDefinition("Bearer", scheme);
    c.AddSecurityRequirement(new OpenApiSecurityRequirement { [scheme] = Array.Empty<string>() });
});

var app = builder.Build();

// Code First: apply pending migrations on boot. Creates the DB if absent
// (Pomelo issues CREATE DATABASE) and is a no-op when the schema is current.
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    db.Database.Migrate();
}

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// Pipeline ORDER matters:
// 1) ErrorHandling -> outermost so any thrown exception is caught.
// 2) CORS          -> before auth so preflight OPTIONS requests succeed.
// 3) Auth/Authz    -> populates User claims.
// 4) RequestLogging-> sits AFTER auth so it can record userId.
// 5) MapControllers-> finally dispatches to controller actions.
app.UseMiddleware<ErrorHandlingMiddleware>();
app.UseCors(CorsPolicy);
app.UseAuthentication();
app.UseAuthorization();
app.UseMiddleware<RequestLoggingMiddleware>();

// NOTE: HTTPS redirection deliberately OFF for local dev (per plan).
// app.UseHttpsRedirection();

app.MapControllers();

app.Run();
