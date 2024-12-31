using Domain.Entities;

namespace App.IRepository;

public interface IUserRepository
{
    Task<User?> GetUser(string email);
}