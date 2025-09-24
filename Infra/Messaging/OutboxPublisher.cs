using System.Text.Json;
using App.Dto;
using Infra.Models;
using OutboxMessage = Infra.Models.OutboxMessage;

namespace Infra.Messaging;

public class OutboxPublisher : IIntegrationEventPublisher
{
    private readonly InfraDbContext _infraDbContext;
    public OutboxPublisher(InfraDbContext infraDbContext) => _infraDbContext = infraDbContext;

    public Task PublishAsync(IntegrationEvent evt, CancellationToken ct = default)
    {
        var json = JsonSerializer.Serialize(evt);
        var msg = new OutboxMessage
        {
            Id = Guid.NewGuid(),
            Type = "user.created",
            Payload = JsonDocument.Parse(json),
            OccurredOn = DateTime.UtcNow
        };

        return _infraDbContext.OutboxMessages.AddAsync(msg, ct).AsTask(); // commit vem no UoW
    }
}   