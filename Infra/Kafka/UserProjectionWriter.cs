/*
 *UserProjectionWriter = aplica eventos → atualiza read model → marca processado.
   Ele é o braço do consumidor Kafka para manter suas projeções de leitura coerentes e idempotentes.
 * 
 *///
using System.Text.Json;
using Infra;
using Infra.Models;
using Microsoft.EntityFrameworkCore;

// Conceito: aplica IntegrationEvents no read-model e marca processed events.
public interface IUserProjectionWriter
{
    Task<bool> AlreadyProcessedAsync(string eventId, CancellationToken ct);
    Task UpsertAsync(Guid userId, string email, CancellationToken ct);
    Task MarkProcessedAsync(string eventId, CancellationToken ct);
}

public sealed class UserProjectionWriter : IUserProjectionWriter
{
    private readonly InfraDbContext _infraDbContext;
    public UserProjectionWriter(InfraDbContext infraDbContext) => _infraDbContext = infraDbContext;

    public Task<bool> AlreadyProcessedAsync(string eventId, CancellationToken ct)
        => _infraDbContext.ProcessedMessages.AnyAsync(p => p.EventId == eventId, ct);

    public async Task UpsertAsync(Guid userId, string email, CancellationToken ct)
    {
        var existing = await _infraDbContext.UsersView.FindAsync(new object?[] { userId }, ct);
        if (existing is null)
            await _infraDbContext.UsersView.AddAsync(new UserView
            {
                Id = userId,
                Email = email,
            }, ct);
        else
            existing.Email = email;
        await _infraDbContext.SaveChangesAsync(ct);
    }

    public async Task MarkProcessedAsync(string eventId, CancellationToken ct)
    {
        await _infraDbContext.ProcessedMessages.AddAsync(new ProcessedMessage { EventId = eventId }, ct);
        await _infraDbContext.SaveChangesAsync(ct);
    }
}