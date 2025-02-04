namespace Domain.Entities;

public enum FriendShipType
{
    Couple,
    CloseFriend,
    Married,
}

public enum FriendShipStatus
{
    Pending,
    Accept,
    Rejected,
    Blocked
}

public class Friends
{
    public Guid Id { get; private set; }
    public Guid UserId01 { get; private set; }
    public Guid UserId02 { get; private set; }
    public FriendShipType Type { get; private set; }
    public FriendShipStatus Status { get; private set; } = FriendShipStatus.Pending;
    public DateTime CreatedAt { get; private set; }

    public Friends(Guid userId01, Guid userId02, FriendShipType type)
    {
        if (userId01 == userId02)
        {
            throw new ArgumentException("Users must be different");
        }
        
        this.Id = Guid.NewGuid();
        this.UserId01 = userId01;
        this.UserId02 = userId02;
        this.Type = type;
        this.CreatedAt = DateTime.Now;
    }

    public void AcceptFriendship()
    {
        if (Status != FriendShipStatus.Pending)
        {
            throw new InvalidOperationException("Friendship can only be accepted if it's pending.");
        }
        this.Status = FriendShipStatus.Accept;
    }
    
    public void RejectFriendship()
    {
        if (Status != FriendShipStatus.Pending)
            throw new InvalidOperationException("Friendship can only be rejected if it's pending.");

        this.Status = FriendShipStatus.Rejected;
    }

    public void RemoveFriendship()
    {
        if (Status == FriendShipStatus.Pending)
            throw new InvalidOperationException("Cannot remove a pending friendship.");
    }
    
    
    internal Friends() {}
    
}