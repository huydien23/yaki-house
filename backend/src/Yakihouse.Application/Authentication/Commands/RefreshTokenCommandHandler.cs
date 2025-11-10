using MediatR;
using Microsoft.Extensions.Logging;
using Yakihouse.Application.Common.Interfaces;
using Yakihouse.Application.Common.Models;
using Yakihouse.Domain.Repositories;

namespace Yakihouse.Application.Authentication.Commands;

public class RefreshTokenCommandHandler : IRequestHandler<RefreshTokenCommand, Result<LoginResponse>>
{
    private readonly IUserRepository _userRepository;
    private readonly IJwtTokenGenerator _jwtTokenGenerator;
    private readonly ILogger<RefreshTokenCommandHandler> _logger;

    public RefreshTokenCommandHandler(
        IUserRepository userRepository,
        IJwtTokenGenerator jwtTokenGenerator,
        ILogger<RefreshTokenCommandHandler> logger)
    {
        _userRepository = userRepository;
        _jwtTokenGenerator = jwtTokenGenerator;
        _logger = logger;
    }

    public async Task<Result<LoginResponse>> Handle(RefreshTokenCommand request, CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(request.RefreshToken))
        {
            _logger.LogWarning("Refresh token request rejected: token missing");
            return Result<LoginResponse>.Failure("Refresh token không hợp lệ");
        }

        var user = await _userRepository.GetByRefreshTokenAsync(request.RefreshToken, cancellationToken);

        if (user is null)
        {
            _logger.LogWarning("Refresh token request rejected: token not found");
            return Result<LoginResponse>.Failure("Refresh token không hợp lệ");
        }

        if (!user.IsActive)
        {
            _logger.LogWarning("Refresh token request rejected: user inactive - {Username}", user.Username);
            return Result<LoginResponse>.Failure("Tài khoản đã bị vô hiệu hóa");
        }

        if (!user.RefreshTokenExpiryTime.HasValue || user.RefreshTokenExpiryTime <= DateTime.UtcNow)
        {
            _logger.LogWarning("Refresh token request rejected: token expired for user {Username}", user.Username);
            return Result<LoginResponse>.Failure("Refresh token đã hết hạn, vui lòng đăng nhập lại");
        }

        if (!string.Equals(user.RefreshToken, request.RefreshToken, StringComparison.Ordinal))
        {
            _logger.LogWarning("Refresh token request rejected: mismatch for user {Username}", user.Username);
            return Result<LoginResponse>.Failure("Refresh token không hợp lệ");
        }

        var newAccessToken = _jwtTokenGenerator.GenerateAccessToken(user);
        var newRefreshToken = _jwtTokenGenerator.GenerateRefreshToken();

        user.RefreshToken = newRefreshToken;
        user.RefreshTokenExpiryTime = DateTime.UtcNow.AddDays(7);
        user.LastLoginAt = DateTime.UtcNow;

        await _userRepository.UpdateAsync(user, cancellationToken);

        var response = new LoginResponse(
            AccessToken: newAccessToken,
            RefreshToken: newRefreshToken,
            ExpiresAt: DateTime.UtcNow.AddHours(1),
            User: new UserDto(user.Id, user.Username, user.FullName, user.Email, user.Role)
        );

        _logger.LogInformation("Refresh token issued for user: {Username}", user.Username);

        return Result<LoginResponse>.Success(response);
    }
}
