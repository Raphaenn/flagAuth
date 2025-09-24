// Conceito: evento para outros bounded contexts/serviços (payload para Kafka).
public abstract record IntegrationEvent(Guid UserId)
{
    public Guid EventId { get; init; } = Guid.NewGuid();
    public DateTime OccurredOn { get; init; } = DateTime.UtcNow;
    public string EventType => GetType().Name;
    public Guid UserId { get; init; } = UserId;
}