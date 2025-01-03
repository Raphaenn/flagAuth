using App.IRepository;
using Domain.Entities;
using Infra.Models;
using Microsoft.EntityFrameworkCore;

namespace Infra.Repository;

public class UserRepository : IUserRepository
{
    private readonly InfraDbContext _infraDbContext;

    public UserRepository(InfraDbContext infraDbContext)
    {
        _infraDbContext = infraDbContext;
    }

    public async Task<User?> GetUser(string email)
    {
        if (_infraDbContext.users_view != null)
        {
            UserView? response = await _infraDbContext.users_view
                .Where(u => u.Email == email)
                .FirstOrDefaultAsync();

            if (response != null)
            {
                return new User(name: response.Name, email: response.Email, userId: "12312");
            }
        }
        return null;
    }
}