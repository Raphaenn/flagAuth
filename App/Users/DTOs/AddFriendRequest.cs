namespace App.Users.DTOs;

public class AddFriendRequest
{
    public Guid UserId01 { get; set; }
    public Guid UserId02 { get; set; }
    public string Type { get; set; }
}