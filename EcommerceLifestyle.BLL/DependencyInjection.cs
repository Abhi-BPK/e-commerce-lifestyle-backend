using EcommerceLifestyle.BLL.Interfaces;
using EcommerceLifestyle.BLL.Services;
using Microsoft.Extensions.DependencyInjection;

namespace EcommerceLifestyle.BLL;

// One-stop shop for BLL DI registrations.
// API project calls services.AddBll() once in Program.cs.
public static class BllServiceCollectionExtensions
{
    public static IServiceCollection AddBll(this IServiceCollection services)
    {
        services.AddScoped<IPasswordHasher, BcryptPasswordHasher>();
        services.AddScoped<ITokenService, TokenService>();
        services.AddScoped<IAuthService, AuthService>();
        services.AddScoped<IProductService, ProductService>();
        services.AddScoped<ICartService, CartService>();
        services.AddScoped<IInventoryService, InventoryService>();
        services.AddScoped<IOrderService, OrderService>();
        services.AddScoped<IUserService, UserService>();
        services.AddScoped<ILogService, LogService>();

        // Vendor dashboard services (additive).
        services.AddScoped<IVendorProductService, VendorProductService>();
        services.AddScoped<IVendorInventoryService, VendorInventoryService>();
        services.AddScoped<IVendorOrderService, VendorOrderService>();
        services.AddScoped<IVendorAnalyticsService, VendorAnalyticsService>();

        // ── External login (Google / GitHub OIDC) ─────────────────────
        // AddHttpClient registers a typed HttpClient per provider so each
        // gets its own connection pool and lifetime management.
        services.AddHttpClient<GoogleOidcClient>();
        services.AddHttpClient<GitHubOAuthClient>();
        services.AddScoped<IExternalLoginService, ExternalLoginService>();

        return services;
    }
}
