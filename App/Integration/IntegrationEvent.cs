// Conceito: evento para outros bounded contexts/serviços (payload para Kafka).

namespace App.Integration;

public abstract record IntegrationEvent
{
    public Guid EventId { get; init; } = Guid.NewGuid(); // id p/ idempotência
    public DateTime OccurredOn { get; init; } = DateTime.UtcNow;
    public string EventType => GetType().Name;           // nome de tipo
}