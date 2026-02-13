using App.Dto;
namespace Infra.Persistence;

public sealed class EfUnitOfWork : IUnitOfWork
{
    private readonly InfraDbContext _db;
    public EfUnitOfWork(InfraDbContext db) => _db = db;

    public Task<int> SaveChangesAsync(CancellationToken ct)
        => _db.SaveChangesAsync(ct);
}
