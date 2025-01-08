using App.Auth.Commands;
using App.IRepository;
using App.Services;
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
            Console.WriteLine(request.Id);
            User userData = UserUseCase.CreateWithExistingId(request.Id, request.Name, request.Email);
            string token = await TokenGenerator.CreateToken(userData);
            await _authRepository.CreateSocialAuth(userData, token);
            return token;

        }
        catch (Exception e)
        {
            throw new Exception("Invalid request");
        }
    }
}