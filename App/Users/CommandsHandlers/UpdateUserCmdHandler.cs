using System.Reflection;
using App.IRepository;
using App.Users.Commands;
using Domain.Entities;
using MediatR;

namespace App.Users.CommandsHandlers;

public class UpdateUserCmdHandler : IRequestHandler<UpdateUserCommand, User>
{
    private readonly IUserRepository _userRepository;
        
    public UpdateUserCmdHandler(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    public async Task<User> Handle(UpdateUserCommand updateUserCommand, CancellationToken cancellationToken)
    {
        User user = await _userRepository.GetUserById(updateUserCommand.Id);

        if (user == null)
        {
            throw new Exception("User not found");
        }

        user.UpdateUser(
            newName: updateUserCommand.Name,
            birth: DateTime.Parse(updateUserCommand.BirthDate), 
            sexualOrientation: updateUserCommand.SexualOrientation,
            sexuality: updateUserCommand.Sexuality,
            country: updateUserCommand.Country,
            city: updateUserCommand.City,
            password: updateUserCommand.Password,
            height: updateUserCommand.Height,
            weight: updateUserCommand.Weight,
            latitude: updateUserCommand.Latitude,
            longitude: updateUserCommand.Longitude
            );
        
        user.ChangeStatus(UserStatus.Incomplete);

        await _userRepository.UpdateUser(user);

        return user;
    }
}