namespace Infra;

public class KafkaOptions
{
    public string? BootstrapServers { get; init; } = Environment.GetEnvironmentVariable("Kafka");
    public string UserTopic { get; init; } = "user-events";
    public bool EnableIdempotence { get; init; } = true;
}