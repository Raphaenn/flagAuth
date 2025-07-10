using App.IRepository;
using App.Users.Queries;
using Domain.Entities;
using MediatR;

namespace App.Users.QueryHandlers;

public class GetUserByIdQHandler : IRequestHandler<GetUserByIdQuery, User>
{
    private readonly IUserRepository _userRepository;

    public GetUserByIdQHandler(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }
    
    public async Task<User> Handle(GetUserByIdQuery request, CancellationToken cancellationToken)
    {
        return await _userRepository.GetUserById(request.Id);
    } 
}