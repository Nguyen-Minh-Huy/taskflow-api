using TaskFlow.Application.DTOs.Auth;

namespace TaskFlow.Application.Interfaces
{
    public interface IAuthService
    {
        System.Threading.Tasks.Task<AuthResponseDto> RegisterAsync(RegisterRequestDto request);
        System.Threading.Tasks.Task<AuthResponseDto> LoginAsync(LoginRequestDto request);
        System.Threading.Tasks.Task<AuthResponseDto> RefreshTokenAsync(RefreshTokenRequestDto request);
    }
}