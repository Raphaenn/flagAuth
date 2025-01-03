using App.Auth.Commands;
using App.IRepository;
using Domain;
using Domain.Entities;
using MediatR;

namespace App.Auth.CommandHandlers;

public class CreateAuthCHandle : IRequestHandler<CreateAuthCommand, string>
{
    private readonly IAuthRepository _authRepository;

    public CreateAuthCHandle(IAuthRepository authRepository)
    {
        _authRepository = authRepository;
    }

    public async Task<string> Handle(CreateAuthCommand request, CancellationToken cancellationToken)
    {
        try
        {
            User userData = UserUseCase.CreateWithExistingId(request.Id, request.Email, request.Email, request.UserId);
            string token = await TokenGenerator.CreateToken(userData);
            await _authRepository.CreateSocialAuth(userData, token);
            return token;

        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw new Exception("Invalid requesrt");
        }
    }
}