using System.ComponentModel.DataAnnotations;

namespace EcommerceLifestyle.BLL.Dtos.Users;

public class UserUpdateDto
{
    [Required, MaxLength(80)]
    public string FirstName { get; set; } = string.Empty;

    [Required, MaxLength(80)]
    public string LastName { get; set; } = string.Empty;
}
