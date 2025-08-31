using App.IRepository;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Infra.Repository;

public class RefreshTokenRepository : IRefreshTokenRepository
{
    private readonly InfraDbContext _infraDbContext;

    public RefreshTokenRepository(InfraDbContext context)
    {
        _infraDbContext = context;
    }

    public async Task SaveAsync(RefreshToken token)
    {
        await _infraDbContext.refreshTokens.AddAsync(token);
        await _infraDbContext.SaveChangesAsync();
    }

    public async Task<RefreshToken?> GetByTokenAsync(string token)
    {
        return await _infraDbContext.refreshTokens
            .FirstOrDefaultAsync(r => r.Token == token && !r.Revoked && r.ExpiresAt > DateTime.UtcNow);
    }

    public async Task InvalidateTokenAsync(string token)
    {
        var refreshToken = await GetByTokenAsync(token);
        if (refreshToken != null)
        {
            refreshToken.Revoked = true;
            await _infraDbContext.SaveChangesAsync();
        }
    }
}
