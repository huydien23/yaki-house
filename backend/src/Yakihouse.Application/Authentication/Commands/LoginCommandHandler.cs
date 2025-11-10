using MediatR;
using Microsoft.Extensions.Logging;
using Yakihouse.Application.Common.Interfaces;
using Yakihouse.Application.Common.Models;
using Yakihouse.Domain.Repositories;

namespace Yakihouse.Application.Authentication.Commands;

public class LoginCommandHandler : IRequestHandler<LoginCommand, Result<LoginResponse>>
{
    private readonly IUserRepository _userRepository;
    private readonly IPasswordHasher _passwordHasher;
    private readonly IJwtTokenGenerator _jwtTokenGenerator;
    private readonly ILogger<LoginCommandHandler> _logger;

    public LoginCommandHandler(
        IUserRepository userRepository,
        IPasswordHasher passwordHasher,
        IJwtTokenGenerator jwtTokenGenerator,
        ILogger<LoginCommandHandler> logger)
    {
        _userRepository = userRepository;
        _passwordHasher = passwordHasher;
        _jwtTokenGenerator = jwtTokenGenerator;
        _logger = logger;
    }

    public async Task<Result<LoginResponse>> Handle(LoginCommand request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Login attempt for username: {Username}", request.Username);

        var user = await _userRepository.GetByUsernameAsync(request.Username, cancellationToken);
        
        if (user == null)
        {
            _logger.LogWarning("Login failed: User not found - {Username}", request.Username);
            return Result<LoginResponse>.Failure("Tên đăng nhập hoặc mật khẩu không đúng");
        }

        if (!user.IsActive)
        {
            _logger.LogWarning("Login failed: User is inactive - {Username}", request.Username);
            return Result<LoginResponse>.Failure("Tài khoản đã bị vô hiệu hóa");
        }

        if (!_passwordHasher.VerifyPassword(request.Password, user.PasswordHash))
        {
            _logger.LogWarning("Login failed: Invalid password - {Username}", request.Username);
            return Result<LoginResponse>.Failure("Tên đăng nhập hoặc mật khẩu không đúng");
        }

        var accessToken = _jwtTokenGenerator.GenerateAccessToken(user);
        var refreshToken = _jwtTokenGenerator.GenerateRefreshToken();

        user.RefreshToken = refreshToken;
        user.RefreshTokenExpiryTime = DateTime.UtcNow.AddDays(7);
        user.LastLoginAt = DateTime.UtcNow;

        await _userRepository.UpdateAsync(user, cancellationToken);

        _logger.LogInformation("Login successful for user: {Username}", request.Username);

        var response = new LoginResponse(
            AccessToken: accessToken,
            RefreshToken: refreshToken,
            ExpiresAt: DateTime.UtcNow.AddHours(1),
            User: new UserDto(user.Id, user.Username, user.FullName, user.Email, user.Role)
        );

        return Result<LoginResponse>.Success(response);
    }
}
