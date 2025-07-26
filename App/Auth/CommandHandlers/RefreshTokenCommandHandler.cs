using Api.Dto;
using App.Auth.Commands;
using App.Dto;
using App.IRepository;
using Domain.Entities;
using MediatR;

namespace App.Auth.CommandHandlers;

public class RefreshTokenCommandHandler : IRequestHandler<RefreshTokenCommand, AuthTokenResponse>
{
    private readonly IRefreshTokenRepository _refreshTokenRepository;
    private readonly ITokenService _tokenService;

    public RefreshTokenCommandHandler(IRefreshTokenRepository refreshTokenRepository, ITokenService tokenService)
    {
        _refreshTokenRepository = refreshTokenRepository;
        _tokenService = tokenService;
    }

    public async Task<AuthTokenResponse> Handle(RefreshTokenCommand request, CancellationToken cancellationToken)
    {
        var token = await _refreshTokenRepository.GetByTokenAsync(request.RefreshToken);

        if (token == null || token.ExpiresAt <= DateTime.UtcNow)
            throw new Exception("Invalid refresh token");

        // Gera novos tokens
        var newAccessToken = _tokenService.GenerateAccessToken(token.UserId);
        var newRefreshToken = _tokenService.GenerateRefreshToken();

        // Invalida o token antigo
        await _refreshTokenRepository.InvalidateTokenAsync(request.RefreshToken);

        // Salva o novo token
        await _refreshTokenRepository.SaveAsync(new RefreshToken()
        {
            UserId = token.UserId,
            Token = newRefreshToken,
            ExpiresAt = DateTime.UtcNow.AddDays(7)
        });

        return new AuthTokenResponse
        {
            AccessToken = newAccessToken,
            RefreshToken = newRefreshToken
        };
    }
}
