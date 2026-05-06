using EcommerceLifestyle.DAL.Entities;
using Microsoft.EntityFrameworkCore;

namespace EcommerceLifestyle.DAL.Persistence;

// EF Core's DbContext is the gateway to the database.
// Each DbSet<T> exposed here becomes a table.
// All schema-shaping happens in OnModelCreating: indexes, FKs, value conversions.
public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    public DbSet<User> Users => Set<User>();
    public DbSet<Product> Products => Set<Product>();
    public DbSet<Cart> CartItems => Set<Cart>();
    public DbSet<Inventory> Inventory => Set<Inventory>();
    public DbSet<Order> Orders => Set<Order>();
    public DbSet<OrderItem> OrderItems => Set<OrderItem>();
    public DbSet<Log> Logs => Set<Log>();

    // Vendor dashboard: per-(Product, Size, Color) stock rows.
    public DbSet<ProductVariant> ProductVariants => Set<ProductVariant>();

    protected override void OnModelCreating(ModelBuilder b)
    {
        base.OnModelCreating(b);

        // ---------- Users ----------
        b.Entity<User>(e =>
        {
            // Unique email -> protects against duplicate signups at the DB level.
            e.HasIndex(u => u.Email).IsUnique();

            // Store enum as readable VARCHAR ("User"/"Vendor") not int.
            e.Property(u => u.Role)
             .HasConversion<string>()
             .HasMaxLength(16);

            // External login (OIDC / OAuth):
            //   - Provider is non-null with a "Local" default so existing rows are valid.
            //   - (Provider, ProviderUserId) must be unique to prevent duplicate
            //     external accounts. Local users keep ProviderUserId = NULL,
            //     and MySQL allows multiple NULLs in a unique index.
            e.Property(u => u.Provider)
             .HasMaxLength(20)
             .HasDefaultValue("Local");

            e.HasIndex(u => new { u.Provider, u.ProviderUserId })
             .IsUnique();
        });

        // ---------- Products ----------
        b.Entity<Product>(e =>
        {
            e.HasIndex(p => p.Slug).IsUnique();
            e.HasIndex(p => p.Subcategory); // GET /api/products/men?subcategory= filter
            e.HasOne<User>()
             .WithMany()
             .HasForeignKey(p => p.VendorId)
             .OnDelete(DeleteBehavior.SetNull);
        });

        // ---------- Cart ----------
        b.Entity<Cart>(e =>
        {
            e.HasIndex(c => new { c.UserId, c.ProductId }).IsUnique();
            e.HasOne<User>()
             .WithMany()
             .HasForeignKey(c => c.UserId)
             .OnDelete(DeleteBehavior.Cascade);
            e.HasOne<Product>()
             .WithMany()
             .HasForeignKey(c => c.ProductId)
             .OnDelete(DeleteBehavior.Cascade);
        });

        // ---------- Inventory ----------
        b.Entity<Inventory>(e =>
        {
            e.HasIndex(i => i.ProductId).IsUnique();
            e.HasOne<Product>()
             .WithMany()
             .HasForeignKey(i => i.ProductId)
             .OnDelete(DeleteBehavior.Cascade);
            e.HasOne<User>()
             .WithMany()
             .HasForeignKey(i => i.VendorId)
             .OnDelete(DeleteBehavior.SetNull);
        });

        // ---------- Orders ----------
        b.Entity<Order>(e =>
        {
            e.Property(o => o.Status)
             .HasConversion<string>()
             .HasMaxLength(16);

            e.HasOne<User>()
             .WithMany()
             .HasForeignKey(o => o.UserId)
             .OnDelete(DeleteBehavior.Cascade);

            // Configure the Order -> OrderItems navigation.
            e.HasMany(o => o.Items)
             .WithOne()
             .HasForeignKey(i => i.OrderId)
             .OnDelete(DeleteBehavior.Cascade);
        });

        // ---------- OrderItems ----------
        b.Entity<OrderItem>(e =>
        {
            // Restrict so we don't accidentally delete a Product that has historic orders.
            e.HasOne<Product>()
             .WithMany()
             .HasForeignKey(i => i.ProductId)
             .OnDelete(DeleteBehavior.Restrict);
        });

        // ---------- Logs ----------
        b.Entity<Log>(e =>
        {
            e.HasIndex(l => l.Timestamp);
        });

        // ---------- ProductVariants (vendor dashboard) ----------
        b.Entity<ProductVariant>(e =>
        {
            // Unique combo: a vendor cannot have two rows for the same
            // (Product, Size, Color) -- they should update stock instead.
            e.HasIndex(v => new { v.ProductId, v.Size, v.Color }).IsUnique();

            e.HasOne<Product>()
             .WithMany()
             .HasForeignKey(v => v.ProductId)
             .OnDelete(DeleteBehavior.Cascade);

            e.HasOne<User>()
             .WithMany()
             .HasForeignKey(v => v.VendorId)
             .OnDelete(DeleteBehavior.SetNull);
        });

        // ---------- Seed ----------
        // Initial product catalogue (translated from frontend mock).
        b.Entity<User>().HasData(SeedData.Users);
        b.Entity<Product>().HasData(SeedData.Products);
        b.Entity<Inventory>().HasData(SeedData.InventoryRows);
    }
}
