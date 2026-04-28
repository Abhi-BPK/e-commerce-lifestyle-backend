namespace EcommerceLifestyle.BLL.Exceptions;

// Domain-aware exception. Carrying StatusCode/Field/Code lets the API
// middleware translate it into the SAME { status, message, field, code }
// JSON shape that the React frontend's errorNormalizer already expects.
public class ApiException : Exception
{
    public int StatusCode { get; }
    public string? Field { get; }
    public string? Code { get; }

    public ApiException(int statusCode, string message, string? field = null, string? code = null)
        : base(message)
    {
        StatusCode = statusCode;
        Field = field;
        Code = code;
    }
}
