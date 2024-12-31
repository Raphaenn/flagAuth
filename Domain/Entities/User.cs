namespace Domain.Entities;

public class User
{
    public string Id { get; set; }
    public string Name { get; set; }
    public string Email { get; set; }
    public string UserId { get; set; }
    
    public User(string name, string email, string userId)
    {
        this.Id = Guid.NewGuid().ToString();
        this.Name = name;
        this.Email = email;
        this.UserId = userId;
    }
}