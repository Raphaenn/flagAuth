using MediatR;

namespace App.Auth.Commands;

public record struct SaveRefreshTokenCommand(Guid UserId, string RefreshToken, DateTime ExpiresAt)
    : IRequest<Unit>;