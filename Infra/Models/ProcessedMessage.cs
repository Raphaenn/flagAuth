// Infrastructure/Persistence/ProcessedMessage.cs
// Conceito: marca eventos já aplicados no read-model (idempotência).
namespace Infra.Models;

public sealed class ProcessedMessage
{
    public string EventId { get; set; } = default!; // PK (string p/ flexibilidade)
    public DateTime ProcessedAt { get; set; } = DateTime.UtcNow;
}