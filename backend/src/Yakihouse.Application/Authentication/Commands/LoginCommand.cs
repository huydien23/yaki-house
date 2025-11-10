using MediatR;
using Yakihouse.Application.Common.Models;

namespace Yakihouse.Application.Authentication.Commands;

public record LoginCommand(
    string Username,
    string Password
) : IRequest<Result<LoginResponse>>;

public record LoginResponse(
    string AccessToken,
    string RefreshToken,
    DateTime ExpiresAt,
    UserDto User
);

public record UserDto(
    Guid Id,
    string Username,
    string FullName,
    string Email,
    string Role
);
