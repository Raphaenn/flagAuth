using Domain.Entities;

namespace Domain;

public static class UserUseCase
{
    public static User CreateNewUser(string name, string email, string userId)
    {
        return new User(name, email);
    }

    public static User CreateWithExistingId(Guid id, string name, string email)
    {
        return new User(name, email);
    }
}