using System.Text;
using App.Users.Integration;
using Confluent.Kafka;
using Confluent.Kafka.Admin;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

// Conceito: consome tópicos, garante idempotência e atualiza read-model.
namespace Infra.Kafka;
public sealed class UserEventsConsumer : BackgroundService
{
    private readonly IServiceScopeFactory _scopeFactory;
    private readonly KafkaOptions _opt;
    private readonly ILogger<UserEventsConsumer> _logger;


    public UserEventsConsumer(IServiceScopeFactory scopeFactory, IOptions<KafkaOptions> opt, ILogger<UserEventsConsumer> logger)
    {
        _scopeFactory = scopeFactory; 
        _opt = opt.Value;
        _logger = logger;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        var cfg = new ConsumerConfig
        {
            BootstrapServers = _opt.BootstrapServers,
            GroupId = "user-service-readmodel-v1", // agrupa instâncias
            AutoOffsetReset = AutoOffsetReset.Earliest,
            EnableAutoCommit = false               // commit manual
        };

        // using var consumer = new ConsumerBuilder<string, string>(cfg).Build();
        IConsumer<string, string>? consumer = null;
        consumer = new ConsumerBuilder<string, string>(cfg)
            .SetErrorHandler((_, e) => _logger.LogWarning("Consumer error: {Reason}", e.Reason))
            .SetPartitionsAssignedHandler((_, parts) =>
                _logger.LogInformation("Assigned: {Parts}", string.Join(",", parts)))
            .SetPartitionsRevokedHandler((_, parts) =>
                _logger.LogInformation("Revoked: {Parts}", string.Join(",", parts)))
            .Build();
        
        await EnsureTopicAsync(_opt.BootstrapServers, _opt.UserTopic, partitions: 6, rf: 1);

        consumer.Subscribe(_opt.UserTopic);

        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {

                var cr = consumer.Consume(stoppingToken);

                var eventType = GetHeader(cr.Message.Headers, "event-type");
                var eventId   = GetHeader(cr.Message.Headers, "event-id");

                using var scope = _scopeFactory.CreateScope();
                var proj = scope.ServiceProvider.GetRequiredService<IUserProjectionWriter>();

                if (await proj.AlreadyProcessedAsync(eventId, stoppingToken))
                {
                    consumer.Commit(cr); // já aplicado → apenas commit
                    continue;
                }

                switch (eventType)
                {
                    case nameof(UserCreatedIntegrationEvent):
                        var evt = System.Text.Json.JsonSerializer
                            .Deserialize<UserCreatedIntegrationEvent>(cr.Message.Value)!;
                        await proj.UpsertAsync(evt.UserId, evt.Email, stoppingToken);
                        break;
                    // outros eventos… (UserUpdated, etc.)
                }

                await proj.MarkProcessedAsync(eventId, stoppingToken); // idempotência
                consumer.Commit(cr);                                   // move offset
            }
            catch (OperationCanceledException) { /* shutdown */ }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao processar mensagem do Kafka.");
            }
        }
    }

    private static string GetHeader(Headers h, string key)
    {
        return h.TryGetLastBytes(key, out var bytes) && bytes is not null
            ? Encoding.UTF8.GetString(bytes)
            : string.Empty; // ou lance uma exceção se o header for obrigatório
    }
    
    public static async Task EnsureTopicAsync(string bootstrap, string topic, int partitions = 6, short rf = 1)
    {
        using var admin = new AdminClientBuilder(new AdminClientConfig { BootstrapServers = bootstrap }).Build();
        try
        {
            await admin.CreateTopicsAsync(new[] {
                new TopicSpecification { Name = topic, NumPartitions = partitions, ReplicationFactor = rf }
            });
        }
        catch (CreateTopicsException ex) when (ex.Results[0].Error.Code == ErrorCode.TopicAlreadyExists)
        {
        }
    }
}
