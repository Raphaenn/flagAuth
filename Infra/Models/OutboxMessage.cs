using System.Text.Json;

namespace Infra.Models;

public sealed class OutboxMessage
{
    public Guid Id { get; set; }
    public DateTime OccurredOn { get; set; }
    public string Type { get; set; } = default!;
    public JsonDocument Payload { get; set; } = default!;  // 👈
    public DateTime? ProcessedOn { get; set; }
    public int Attempts { get; set; }              // ← contagem de tentativas
    public DateTime? NextAttemptAt { get; set; }   // ← quando tentar de novo
    public string? Error { get; set; }  
}