// Conceito: "publicar" aqui significa gravar na tabela Outbox, não no Kafka direto.

using App.Integration;

namespace App.Dto;

public interface IIntegrationEventPublisher {
    
    Task PublishAsync(IntegrationEvent evt, CancellationToken ct = default);
}