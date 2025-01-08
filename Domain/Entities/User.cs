namespace Domain.Entities;

public class User
{
    public string Id { get; set; }
    public string Name { get; set; }
    public string Email { get; set; }
    
    public User(string id, string name, string email)
    {
        this.Id = id;
        this.Name = name;
        this.Email = email;
    }

    internal User(string name, string email)
    {
        this.Name = name;
        this.Email = email;
    }
}