using Confluent.Kafka;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using System.Text.Json;
using Infra;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

// Conceito: processo em background que busca mensagens pendentes na Outbox
// e publica no Kafka com cabeçalhos para dedupe/roteamento.
public sealed class OutboxDispatcher : BackgroundService
{
    private readonly IServiceScopeFactory _scopeFactory;
    private const int BatchSize = 100;
    private readonly IKafkaProducer _producer;
    private readonly KafkaOptions _kafkaOptions;
    private readonly ILogger<OutboxDispatcher> _logger;


    public OutboxDispatcher(IServiceScopeFactory scopeFactory, IKafkaProducer producer, IOptions<KafkaOptions> kafkaOptions, ILogger<OutboxDispatcher> logger)
    {
        _scopeFactory = scopeFactory; 
        _producer = producer; 
        _kafkaOptions = kafkaOptions.Value;
        _logger = logger;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _logger.LogInformation("OutboxDispatcher started");

        while (!stoppingToken.IsCancellationRequested)
        {
            using var scope = _scopeFactory.CreateScope();
            var db = scope.ServiceProvider.GetRequiredService<InfraDbContext>();

            var batch = await db.OutboxMessages
                .AsQueryable()
                .Where(x => x.ProcessedOn == null &&
                            (x.NextAttemptAt == null || x.NextAttemptAt <= DateTime.UtcNow))
                .OrderBy(x => x.OccurredOn)
                .Take(100)
                .ToListAsync(stoppingToken);

            if (batch.Count == 0)
            {
                await Task.Delay(5000, stoppingToken);
                continue;
            }

            foreach (var msg in batch)
            {
                try
                {
                    // 1) Converter o payload para string
                    object payloadObj = msg.Payload; // força ser object
                    string payload = payloadObj switch
                    {
                        string s => s,
                        JsonDocument jd => JsonSerializer.Serialize(jd.RootElement),
                        JsonElement je => JsonSerializer.Serialize(je),
                        _ => msg.Payload?.ToString() ?? "{}"
                    };

                    // 2) Particionamento: tenta extrair uma chave (UserId) do JSON
                    var key = TryExtractKey(payload) ?? msg.Id.ToString();
                    
                    // 3) Cabeçalhos úteis
                    var headers = new Headers
                    {
                        new Header("event-id", System.Text.Encoding.UTF8.GetBytes(msg.Id.ToString())),
                        new Header("event-type", System.Text.Encoding.UTF8.GetBytes(msg.Type)),
                        new Header("occurred-on", System.Text.Encoding.UTF8.GetBytes(msg.OccurredOn.ToUniversalTime().ToString("O")))
                    };
                    
                    // 4) Publica no Kafka usando o SEU IKafkaProducer (string payload)
                    await _producer.ProduceAsync(
                        topic: _kafkaOptions.UserTopic,
                        key: key,
                        payload: payload,
                        headers: headers,
                        ct: stoppingToken);
                    
                    msg.ProcessedOn = DateTime.UtcNow;
                    msg.Error = null;
                }
                catch (Exception ex)
                {
                    msg.Attempts++;
                    msg.Error = ex.Message;
                    msg.NextAttemptAt = DateTime.UtcNow.AddSeconds(15 * Math.Pow(2, msg.Attempts));
                }
            }

            await db.SaveChangesAsync(stoppingToken);
            await Task.Delay(50, stoppingToken); // pequeno respiro entre lotes
        }
        _logger.LogInformation("OutboxDispatcher stopped");
    }

    private static string? TryExtractKey(string payload)
    {
        // tenta pegar UserId do JSON para usar como chave de partição
        try
        {
            using var doc = System.Text.Json.JsonDocument.Parse(payload);
            if (doc.RootElement.TryGetProperty("UserId", out var id)) return id.GetGuid().ToString();
        }
        catch
        {
            // throw new Exception("Invalid payload");
            throw new Exception("Invalid payload");
        }
        return null;
    }
}
