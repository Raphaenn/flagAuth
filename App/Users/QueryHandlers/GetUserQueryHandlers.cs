using App.IRepository;
using App.Users.Queries;
using Domain.Entities;
using MediatR;

namespace App.Users.QueryHandlers;

public class GetUserQueryHandlers : IRequestHandler<GetUserQuery, User?>
{
    private readonly IUserRepository _userRepository;

    public GetUserQueryHandlers(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    public async Task<User?> Handle(GetUserQuery request, CancellationToken cancellationToken)
    {
        User? user = await _userRepository.GetUser(request.Email);

        if (user == null)
        {
            return null;
        }

        return user;
    }
}