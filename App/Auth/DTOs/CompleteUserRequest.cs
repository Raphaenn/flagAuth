using Domain.Entities;

namespace App.Auth.DTOs;

public class CompleteUserRequest
{
    public string Id { get; set; }
    public string Name  { get; set; }
    public string BrithDate  { get; set; }
    public string Country { get; set; }
    public string City { get; set; }
    public Sexualities Sexuality { get; set; }
    public SexualOrientations SexualOrientation { get; set; }
    public string Password { get; set; }
}