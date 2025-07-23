using Domain.Entities;

namespace App.IRepository;

public interface IUserRepository
{
    Task<User?> GetUser(string email);
    
    Task<User> GetUserById(string id);

    Task<Guid> CreateUser(User user);

    Task<Boolean> UpdateUser(User user);

    Task ChangeUserStatus(User user);
}