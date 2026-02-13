using App.Auth.Commands;
using App.IRepository;
using App.Services;
using Domain;
using Domain.Entities;
using Domain.Factory;
using MediatR;

namespace App.Auth.CommandHandlers;

public class CompletedAuthCmdHandle : IRequestHandler<CompletedAuthCommand, string>
{
    private readonly IAuthRepository _authRepository;

    public CompletedAuthCmdHandle(IAuthRepository authRepository)
    {
        _authRepository = authRepository;
    }

    public async Task<string> Handle(CompletedAuthCommand request, CancellationToken cancellationToken)
    {
        try
        {
            string token = await TokenGenerator.CreateToken(request.User);
            await _authRepository.CreateSocialAuth(request.User, token);
            return token;
        }
        catch (Exception e)
        {
            throw new Exception(e.Message);
        }
    }
}