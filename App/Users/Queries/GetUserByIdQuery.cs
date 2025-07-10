using Domain.Entities;
using MediatR;

namespace App.Users.Queries;

public class GetUserByIdQuery : IRequest<User>
{
    public string Id { get; }

    public GetUserByIdQuery(string id)
    {
        Id = id;
    }
}