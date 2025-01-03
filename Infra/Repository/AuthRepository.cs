using App.IRepository;
using Domain.Entities;
using Infra.Models;
using Microsoft.EntityFrameworkCore;

namespace Infra.Repository;

public class AuthRepository : IAuthRepository
{
    private readonly InfraDbContext _infraDbContext;

    public AuthRepository(InfraDbContext infraDbContext)
    {
        _infraDbContext = infraDbContext;
    }

    // public async Task<Auth> CreateAuth(string token, string userId)
    // {
    //     Login login = new Login
    //     {
    //         Id = null,
    //         Token = null,
    //         UserId = null,
    //         ExpireAt = default,
    //         CreatedAt = default
    //     };
    //     await _infraDbContext.login.AddAsync(login);
    //     await _infraDbContext.SaveChangesAsync();
    //     
    //     Auth response = new Auth(token: token, userId: userId, createdAt: DateTime.Now, expireAt: DateTime.Now);
    //     return response;
    // }

    public async Task CreateSocialAuth(User user, string token)
    {
        try
        {
            Login login = new Login
            {
                Id = null,
                Token = null,
                UserId = null,
                ExpireAt = default,
                CreatedAt = default
            };
            await _infraDbContext.login.AddAsync(login);
            await _infraDbContext.SaveChangesAsync();
        }
        catch
        {
            throw new Exception("Db error");
        }
    }
}