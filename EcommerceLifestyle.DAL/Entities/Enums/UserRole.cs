namespace EcommerceLifestyle.DAL.Entities.Enums;

// Two roles supported by the application.
// Stored in MySQL as VARCHAR(16) (see AppDbContext.OnModelCreating)
// so DB rows are human-readable instead of opaque integers.
public enum UserRole
{
    User,
    Vendor
}
