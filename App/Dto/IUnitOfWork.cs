// Conceito: orquestra um commit único do caso de uso (write model + outbox).

namespace App.Dto;

public interface IUnitOfWork
{
    Task<int> SaveChangesAsync(CancellationToken ct = default);
}