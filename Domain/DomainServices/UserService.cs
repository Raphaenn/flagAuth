using Domain.Entities;

namespace Domain.DomainServices;

public class UserService
{
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