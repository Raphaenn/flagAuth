using MediatR;

namespace App.Users.Commands;

public record struct ChangePasswordCommand(Guid UserId, string OldPassword, string NewPassword) : IRequest<bool>;