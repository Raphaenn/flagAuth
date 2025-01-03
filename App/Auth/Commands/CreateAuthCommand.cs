using MediatR;

namespace App.Auth.Commands;

public class CreateAuthCommand : IRequest<string>
{
    public string Id { get; set; }
    public string Email { get; set; }
    public string UserId { get; set; }
}