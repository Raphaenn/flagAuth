using Domain.Entities;
using MediatR;

namespace App.Users.Queries;

public class GetUserQuery : IRequest<User>
{
    public string? UserId { get; set; }
    public string? Email { get; set; }
}