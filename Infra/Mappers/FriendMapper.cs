using Domain.Entities;
using Infra.Models;

namespace Infra.Mappers;

public static class FriendMapper
{
    public static FriendsDbModel ToEntity(Friends domain)
    {
        return new FriendsDbModel
        {
            Id = domain.Id,
            UserId01 = domain.UserId01,
            UserId02 = domain.UserId02,
            Type = domain.Type.ToString(),
            Status = domain.Status.ToString(),
            CreatedAt = domain.CreatedAt
        };
    }

    public static Friends ToDomain(FriendsDbModel entity)
    {
        return new Friends(entity.UserId01, entity.UserId02,
            Enum.Parse<FriendShipType>(entity.Type));
    }
}