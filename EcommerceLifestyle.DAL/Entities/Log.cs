using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EcommerceLifestyle.DAL.Entities;

// Application audit log. Written to by:
//  - RequestLoggingMiddleware (every non-swagger HTTP request)
//  - ErrorHandlingMiddleware  (every unhandled exception)
[Table("Logs")]
public class Log
{
    [Key]
    public long Id { get; set; }

    public DateTime Timestamp { get; set; } = DateTime.UtcNow;

    [Required, MaxLength(16)]
    public string Level { get; set; } = "Info"; // Info | Warn | Error

    [Required, MaxLength(80)]
    public string Source { get; set; } = string.Empty;

    [Required, MaxLength(1000)]
    public string Message { get; set; } = string.Empty;

    public int? UserId { get; set; }

    [MaxLength(500)]
    public string? RequestPath { get; set; }

    [Column(TypeName = "TEXT")]
    public string? Exception { get; set; }
}
