using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using TaskFlow.Application.Commons.Exceptions;
using TaskFlow.Application.DTOs.Auth;
using TaskFlow.Application.Interfaces;
using TaskFlow.Domain.Entities;
namespace TaskFlow.Infrastructure.Services;

public class AuthService : IAuthService
{
    private readonly TaskFlowDbContext _context;
    private readonly IMapper _mapper;
    private readonly ITokenService _TokenService;
    private readonly IConfiguration _configuration;
    public AuthService(
        TaskFlowDbContext context,
        IMapper mapper,
        ITokenService TokenService,
        IConfiguration configuration)
    {
        _context = context;
        _mapper = mapper;
        _TokenService = TokenService;
        _configuration = configuration;
    }
    public async System.Threading.Tasks.Task<AuthResponseDto> RegisterAsync(RegisterRequestDto request)
    {
        // 1. Kiểm tra email đã tồn tại
        var existingUser = await _context.Users
            .FirstOrDefaultAsync(u => u.Email == request.Email);
        if (existingUser != null)
        {
            throw new BusinessException("Email đã được sử dụng");
        }
        // 2. Tạo User entity
        var user = _mapper.Map<User>(request);
        user.Id = Guid.NewGuid();
        user.PasswordHash = BCrypt.Net.BCrypt.HashPassword(request.Password);
        // 3. Lưu User vào database
        _context.Users.Add(user);
        await _context.SaveChangesAsync();
        // 4. Generate tokens
        var accessToken = _TokenService.GenerateAccessToken(user);
        var refreshTokenString = _TokenService.GenerateRefreshToken();
        // 5. Lưu RefreshToken vào database
        var refreshToken = new RefreshToken
        {
            Id = Guid.NewGuid(),
            UserId = user.Id,
            Token = refreshTokenString,
            ExpiresAt = DateTime.UtcNow.AddDays(
                int.Parse(_configuration["Jwt:RefreshTokenExpirationDays"]!)),
            IsRevoked = false
        };
        _context.RefreshTokens.Add(refreshToken);
        await _context.SaveChangesAsync();
        // 6. Trả về response
        return new AuthResponseDto
        {
            AccessToken = accessToken,
            RefreshToken = refreshTokenString,
            ExpiresAt = DateTime.UtcNow.AddMinutes(
                int.Parse(_configuration["Jwt:AccessTokenExpirationMinutes"]!)),
            User = _mapper.Map<UserDto>(user)
        };
    }
    public async System.Threading.Tasks.Task<AuthResponseDto> LoginAsync(LoginRequestDto request)
    {
        // 1. Tìm user theo email
        var user = await _context.Users
            .FirstOrDefaultAsync(u => u.Email == request.Email);
        if (user == null)
        {
            throw new UnauthorizedException("Email hoặc mật khẩu không đúng");
        }
        // 2. Verify password
        if (!BCrypt.Net.BCrypt.Verify(request.Password, user.PasswordHash))
        {
            throw new UnauthorizedException("Email hoặc mật khẩu không đúng");
        }
        // 3. Revoke tất cả refresh tokens cũ của user
        var oldTokens = await _context.RefreshTokens
            .Where(rt => rt.UserId == user.Id && !rt.IsRevoked)
            .ToListAsync();
        foreach (var oldToken in oldTokens)
        {
            oldToken.IsRevoked = true;
            oldToken.RevokedReason = "Đăng nhập mới";
        }
        // 4. Generate tokens mới
        var accessToken = _TokenService.GenerateAccessToken(user);
        var refreshTokenString = _TokenService.GenerateRefreshToken();
        // 5. Lưu RefreshToken mới
        var refreshToken = new RefreshToken
        {
            Id = Guid.NewGuid(),
            UserId = user.Id,
            Token = refreshTokenString,
            ExpiresAt = DateTime.UtcNow.AddDays(
                int.Parse(_configuration["Jwt:RefreshTokenExpirationDays"]!)),
            IsRevoked = false
        };
        _context.RefreshTokens.Add(refreshToken);
        await _context.SaveChangesAsync();
        // 6. Trả về response
        return new AuthResponseDto
        {
            AccessToken = accessToken,
            RefreshToken = refreshTokenString,
            ExpiresAt = DateTime.UtcNow.AddMinutes(
                int.Parse(_configuration["Jwt:AccessTokenExpirationMinutes"]!)),
            User = _mapper.Map<UserDto>(user)
        };
    }
    public async System.Threading.Tasks.Task<AuthResponseDto> RefreshTokenAsync(RefreshTokenRequestDto request)
    {
        // 1. Tìm refresh token trong database
        var refreshToken = await _context.RefreshTokens
            .Include(rt => rt.User)
            .FirstOrDefaultAsync(rt => rt.Token == request.RefreshToken);
        if (refreshToken == null)
        {
            throw new UnauthorizedException("Refresh token không hợp lệ");
        }
        // 2. Kiểm tra token đã bị revoke chưa
        if (refreshToken.IsRevoked)
        {
            throw new UnauthorizedException("Refresh token đã bị thu hồi");
        }
        // 3. Kiểm tra token đã hết hạn chưa
        if (refreshToken.ExpiresAt < DateTime.UtcNow)
        {
            throw new UnauthorizedException("Refresh token đã hết hạn");
        }
        // 4. Revoke token cũ
        refreshToken.IsRevoked = true;
        refreshToken.RevokedReason = "Đã sử dụng để refresh";
        // 5. Generate tokens mới
        var accessToken = _TokenService.GenerateAccessToken(refreshToken.User);
        var newRefreshTokenString = _TokenService.GenerateRefreshToken();
        // 6. Lưu RefreshToken mới
        var newRefreshToken = new RefreshToken
        {
            Id = Guid.NewGuid(),
            UserId = refreshToken.UserId,
            Token = newRefreshTokenString,
            ExpiresAt = DateTime.UtcNow.AddDays(
                int.Parse(_configuration["Jwt:RefreshTokenExpirationDays"]!)),
            IsRevoked = false
        };
        _context.RefreshTokens.Add(newRefreshToken);
        await _context.SaveChangesAsync();
        // 7. Trả về response
        return new AuthResponseDto
        {
            AccessToken = accessToken,
            RefreshToken = newRefreshTokenString,
            ExpiresAt = DateTime.UtcNow.AddMinutes(
                int.Parse(_configuration["Jwt:AccessTokenExpirationMinutes"]!)),
            User = _mapper.Map<UserDto>(refreshToken.User)
        };
    }
}