using Domain.Entities;

namespace Domain;

public static class UserUseCase
{
    public static User CreateNewUser(string name, string email, string userId)
    {
        return new User(name, email, userId);
    }

    public static User CreateWithExistingId(string id, string name, string email)
    {
        return new User(id, name, email);
    }
}