using MediatR;

namespace App.Auth.Commands;

public class CreateAuthCommand : IRequest<string>
{
    public Guid Id { get; set; }
    public string Email { get; set; }
    public string Name { get; set; }
}