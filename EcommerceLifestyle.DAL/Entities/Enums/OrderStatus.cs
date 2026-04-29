namespace EcommerceLifestyle.DAL.Entities.Enums;

// Order lifecycle states.
//
// Stored in MySQL as VARCHAR(16) via HasConversion<string>(),
// so adding new values here is a non-breaking change for existing rows
// (they still hold "Processing"/"Shipped"/"Delivered" strings).
//
// The PUBLIC /api/orders endpoint serializes these as lowercase
// ("processing"/"shipped"/"delivered") for the customer frontend.
//
// The VENDOR /api/vendor/orders endpoint serializes them as PascalCase
// ("Pending"/"Processing"/"Shipped"/"Delivered"/"Cancelled") to match
// the vendor dashboard's tab labels.
public enum OrderStatus
{
    // Existing values -- DO NOT REORDER (preserves int ordinals if anyone
    // ever cast to int historically).
    Processing,
    Shipped,
    Delivered,

    // Added for the vendor dashboard.
    Pending,
    Cancelled
}
