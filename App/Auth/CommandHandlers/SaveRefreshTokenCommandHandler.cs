using App.Auth.Commands;
using App.IRepository;
using Domain.Entities;
using MediatR;

namespace App.Auth.CommandHandlers;

public class SaveRefreshTokenCommandHandler : IRequestHandler<SaveRefreshTokenCommand, Unit>
{
    private readonly IRefreshTokenRepository _repository;

    public SaveRefreshTokenCommandHandler(IRefreshTokenRepository repository)
    {
        _repository = repository;
    }

    public async Task<Unit> Handle(SaveRefreshTokenCommand request, CancellationToken cancellationToken)
    {
        var token = new RefreshToken
        {
            UserId = request.UserId,
            Token = request.RefreshToken,
            ExpiresAt = request.ExpiresAt
        };

        await _repository.SaveAsync(token);
        return Unit.Value;
    }
}
