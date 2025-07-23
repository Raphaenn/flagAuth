using Domain.Entities;

namespace Domain;

public static class UserUseCase
{
    public static User CreateNewUser(string name, string email)
    {
        return User.Create(email: email, name, null, null, null, null, null, null, null, null, null, null, UserStatus.Incomplete);
    }

    public static User CreateWithExistingId(Guid id, string email)
    {
        return User.Rehydrate(id: id,
            email: email,
            name: null,
            birthdate: null,
            country: null,
            city: null,
            sexuality: null,
            sexualOrientation: null,
            password: null,
            height: null, 
            weight: null, 
            latitude: null, 
            longitude: null,
            status: UserStatus.Incomplete
            );
    }
}