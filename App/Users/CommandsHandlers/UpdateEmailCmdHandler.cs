using System.Reflection;
using App.Dto;
using App.IRepository;
using App.Users.Commands;
using Domain.Entities;
using MediatR;

namespace App.Users.CommandsHandlers;

public class UpdateEmailCmdHandler : IRequestHandler<UpdateEmailCommand, bool>
{
    private readonly IUserRepository _usersRepository;
    private readonly IUnitOfWork _uow;

    public UpdateEmailCmdHandler(IUserRepository usersRepository, IUnitOfWork unitOfWork)
    {
        _usersRepository = usersRepository;
        _uow = unitOfWork;
    }

    public async Task<bool> Handle(UpdateEmailCommand cmd, CancellationToken ct)
    {
        User user = await _usersRepository.GetUserById(cmd.UserId.ToString())
                    ?? throw new ArgumentException("User not found");

        user.ChangeEmail(cmd.NewEmail);

        await _usersRepository.UpdateUser(user);

        return true;
    }
}

    