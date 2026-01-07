using Microsoft.AspNetCore.Mvc;
using TaskFlow.Application.Commons;
using TaskFlow.Application.DTOs.Auth;
using TaskFlow.Application.Interfaces;


namespace TaskFlow.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly IAuthService _authService;
    public AuthController(IAuthService authService)
    {
        _authService = authService;
    }
    [HttpPost("register")]
    public async System.Threading.Tasks.Task<IActionResult> Register([FromBody] RegisterRequestDto request)
    {
        var result = await _authService.RegisterAsync(request);
        return Ok(ApiResponse<AuthResponseDto>.CreateSuccess(
            data: result,
            message: "Đăng ký thành công",
            statusCode: 201
        ));
    }
    [HttpPost("login")]
    public async System.Threading.Tasks.Task<IActionResult> Login([FromBody] LoginRequestDto request)
    {
        var result = await _authService.LoginAsync(request);
        return Ok(ApiResponse<AuthResponseDto>.CreateSuccess(
            data: result,
            message: "Đăng nhập thành công"
        ));
    }
    [HttpPost("refresh")]
    public async System.Threading.Tasks.Task<IActionResult> RefreshToken([FromBody] RefreshTokenRequestDto request)
    {
        var result = await _authService.RefreshTokenAsync(request);
        return Ok(ApiResponse<AuthResponseDto>.CreateSuccess(
            data: result,
            message: "Làm mới token thành công"
        ));
    }
}