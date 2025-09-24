using App.IRepository;
using Domain.Entities;
using Infra.Mappers;
using Infra.Models;

namespace Infra.Repository;

public class FriendRepository : IFriendRepository
{
    private readonly InfraDbContext _infraDbContext;

    public FriendRepository(InfraDbContext infraDbContext)
    {
        _infraDbContext = infraDbContext;
    } 

    public async Task<Guid> AddFriend(Friends friends)
    {
        FriendsDbModel entity = FriendMapper.ToEntity(friends);
        await _infraDbContext.Friends.AddAsync(entity);
        await _infraDbContext.SaveChangesAsync();
        return friends.Id;
    }
}