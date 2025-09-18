// Conceito: registra evento no momento da criação de usuário.

using Domain.Abstractions;

namespace Domain.Events;
public sealed record UserRegistered(Guid UserId, string Email) : IDomainEvents
{
    public DateTime OccurredOn { get; } = DateTime.UtcNow;
}