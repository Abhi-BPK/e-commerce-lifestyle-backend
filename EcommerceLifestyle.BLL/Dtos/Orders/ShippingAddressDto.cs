using System.ComponentModel.DataAnnotations;

namespace EcommerceLifestyle.BLL.Dtos.Orders;

public class ShippingAddressDto
{
    [Required] public string FirstName { get; set; } = string.Empty;
    [Required] public string LastName  { get; set; } = string.Empty;
    [Required] public string Line1     { get; set; } = string.Empty;
    [Required] public string City      { get; set; } = string.Empty;
    [Required] public string State     { get; set; } = string.Empty;
    [Required] public string Zip       { get; set; } = string.Empty;
    [Required] public string Country   { get; set; } = string.Empty;
}
