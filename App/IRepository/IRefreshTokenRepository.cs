using Domain.Entities;

namespace App.IRepository;

public interface IRefreshTokenRepository
{
    Task SaveAsync(RefreshToken token);
    Task<RefreshToken?> GetByTokenAsync(string token);
    Task InvalidateTokenAsync(string token);
}