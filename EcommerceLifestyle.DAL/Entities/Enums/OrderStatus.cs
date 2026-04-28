namespace EcommerceLifestyle.DAL.Entities.Enums;

// Order lifecycle states. Match the strings the React frontend expects
// ("processing" | "shipped" | "delivered") via camelCase serialization.
public enum OrderStatus
{
    Processing,
    Shipped,
    Delivered
}
