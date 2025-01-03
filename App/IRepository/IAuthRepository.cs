using Domain.Entities;

namespace App.IRepository;

public interface IAuthRepository
{
    Task CreateSocialAuth(User user, string token);
    // Task<Domain.Entities.Auth> CreateAuth(string email, string password);
}