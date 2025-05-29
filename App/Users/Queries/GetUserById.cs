using Domain.Entities;
using MediatR;

namespace App.Users.Queries;

public class GetUserById : IRequest<User>
{
    public string Id { get; set; }
}