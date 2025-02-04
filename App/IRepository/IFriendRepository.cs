using Domain.Entities;

namespace App.IRepository;

public interface IFriendRepository
{
    Task<Guid> AddFriend(Friends friend);
}