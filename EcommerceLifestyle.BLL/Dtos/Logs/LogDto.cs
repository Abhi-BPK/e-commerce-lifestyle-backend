namespace EcommerceLifestyle.BLL.Dtos.Logs;

public class LogDto
{
    public long Id { get; set; }
    public DateTime Timestamp { get; set; }
    public string Level { get; set; } = string.Empty;
    public string Source { get; set; } = string.Empty;
    public string Message { get; set; } = string.Empty;
    public int? UserId { get; set; }
    public string? RequestPath { get; set; }
    public string? Exception { get; set; }
}
