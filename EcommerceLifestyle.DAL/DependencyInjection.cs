using EcommerceLifestyle.DAL.Interfaces;
using EcommerceLifestyle.DAL.Persistence;
using EcommerceLifestyle.DAL.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace EcommerceLifestyle.DAL;

// Centralised DI for the data layer.
// API project calls services.AddDal(connStr) once in Program.cs.
public static class DalServiceCollectionExtensions
{
    public static IServiceCollection AddDal(this IServiceCollection services, string connectionString)
    {
        // Pinned to MySQL 8.0 (the local server we run against, see README).
        // Pomelo speaks both MySQL and MariaDB; we just have to tell it which.
        // We pin a fixed version because AutoDetect would force a live DB
        // connection at design-time (migrations) -- which breaks `dotnet ef`
        // if the server is offline.
        var serverVersion = new MySqlServerVersion(new Version(8, 0, 0));

        services.AddDbContext<AppDbContext>(options =>
            options.UseMySql(connectionString, serverVersion));

        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<IProductRepository, ProductRepository>();
        services.AddScoped<ICartRepository, CartRepository>();
        services.AddScoped<IInventoryRepository, InventoryRepository>();
        services.AddScoped<IOrderRepository, OrderRepository>();
        services.AddScoped<ILogRepository, LogRepository>();
        services.AddScoped<IUnitOfWork, UnitOfWork>();

        return services;
    }
}
