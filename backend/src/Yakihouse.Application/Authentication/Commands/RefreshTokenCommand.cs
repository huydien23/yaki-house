using MediatR;
using Yakihouse.Application.Common.Models;

namespace Yakihouse.Application.Authentication.Commands;

public record RefreshTokenCommand(
    string AccessToken,
    string RefreshToken
) : IRequest<Result<LoginResponse>>;
