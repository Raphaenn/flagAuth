using Domain.Entities;
using MediatR;

namespace App.Users.Queries;

public class GetUserQuery : IRequest<User>
{
    public string Email { get; set; } = String.Empty;
}