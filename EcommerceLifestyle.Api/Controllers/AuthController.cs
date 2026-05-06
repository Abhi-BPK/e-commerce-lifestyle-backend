using EcommerceLifestyle.BLL.Dtos.Auth;
using EcommerceLifestyle.BLL.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EcommerceLifestyle.Api.Controllers;

[ApiController]
[Route("api/auth")]
public class AuthController : ControllerBase
{
    private readonly IAuthService _auth;
    private readonly IExternalLoginService _external;

    public AuthController(IAuthService auth, IExternalLoginService external)
    {
        _auth = auth;
        _external = external;
    }

    // POST /api/auth/login -> { token, user }
    [AllowAnonymous]
    [HttpPost("login")]
    public async Task<ActionResult<LoginResponse>> Login([FromBody] LoginRequest req, CancellationToken ct)
        => Ok(await _auth.LoginAsync(req, ct));

    // POST /api/auth/signup -> { success: true }
    [AllowAnonymous]
    [HttpPost("signup")]
    public async Task<IActionResult> Signup([FromBody] SignupRequest req, CancellationToken ct)
    {
        await _auth.SignupAsync(req, ct);
        return Ok(new { success = true });
    }

    // POST /api/auth/oidc/google -> { token, user }
    // Body: { code, redirectUri, codeVerifier? }
    // SPA exchange: the frontend completes the redirect dance with Google,
    // then forwards the resulting authorization code + the redirect_uri it used.
    [AllowAnonymous]
    [HttpPost("oidc/google")]
    public async Task<ActionResult<LoginResponse>> GoogleLogin([FromBody] OidcLoginRequest req, CancellationToken ct)
        => Ok(await _external.LoginAsync("Google", req, ct));

    // POST /api/auth/oidc/github -> { token, user }
    // Body: { code, redirectUri }
    [AllowAnonymous]
    [HttpPost("oidc/github")]
    public async Task<ActionResult<LoginResponse>> GitHubLogin([FromBody] OidcLoginRequest req, CancellationToken ct)
        => Ok(await _external.LoginAsync("GitHub", req, ct));
}
