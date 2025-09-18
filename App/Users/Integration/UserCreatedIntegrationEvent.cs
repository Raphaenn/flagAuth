// Conceito: versão "externa" do evento, com semântica de negócio.

namespace App.Users.Integration;

public sealed record UserCreatedIntegrationEvent(Guid UserId, string Email) : IntegrationEvent(UserId);