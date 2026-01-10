using App.Dto;
using App.IRepository;
using App.Users.Commands;
using Domain.Entities;
using MediatR;

namespace App.Users.CommandsHandlers;

public class UpdateUserNameCmdHandler : IRequestHandler<UpdateUserNameCommand, bool>

{
    private readonly IUserRepository _usersRepository;
    private readonly IUnitOfWork _uow;

    public UpdateUserNameCmdHandler(IUserRepository usersRepository, IUnitOfWork unitOfWork)
    {
        _usersRepository = usersRepository;
        _uow = unitOfWork;
    }

    public async Task<bool> Handle(UpdateUserNameCommand cmd, CancellationToken ct)
    {
        User user = await _usersRepository.GetUserById(cmd.UserId.ToString())
                   ?? throw new ArgumentException("User not found");

        user.ChangeName(cmd.NewName);

        await _usersRepository.UpdateUser(user);

        return true;
    }
}